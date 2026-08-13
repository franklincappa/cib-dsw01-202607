import { useEffect, useRef, useState, useCallback } from 'react'
import * as signalR from '@microsoft/signalr'
import { HUB_URL } from '../config'

/**
 * Hook que abstrae la conexión a SignalR.
 *
 * Devuelve:
 *  - status: 'idle' | 'connecting' | 'connected' | 'disconnected' | 'error'
 *  - messages: ChatMessage[]
 *  - usersOnline: ConnectedUser[]
 *  - typingUsers: string[]
 *  - sendMessage(content)
 *  - sendPrivateMessage(toUsername, content)
 *  - notifyTyping(isTyping)
 */
export function useSignalR(token, currentUsername) {
  const connectionRef = useRef(null)
  const [status, setStatus] = useState('idle')
  const [messages, setMessages] = useState([])
  const [usersOnline, setUsersOnline] = useState([])
  const [typingUsers, setTypingUsers] = useState([])

  useEffect(() => {
    if (!token) return

    setStatus('connecting')

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_URL, {
        // El backend lee el JWT desde la query string (?access_token=...)
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build()

    connectionRef.current = connection

    // ===== Listeners de eventos del servidor =====
    connection.on('ReceiveMessage', (message) => {
      setMessages((prev) => [...prev, message])
    })

    connection.on('UserConnected', (message) => {
      setMessages((prev) => [...prev, message])
    })

    connection.on('UserDisconnected', (message) => {
      setMessages((prev) => [...prev, message])
    })

    connection.on('UsersOnline', (users) => {
      setUsersOnline(users)
    })

    connection.on('UserTyping', ({ user, isTyping }) => {
      setTypingUsers((prev) => {
        if (isTyping) {
          return prev.includes(user) ? prev : [...prev, user]
        }
        return prev.filter((u) => u !== user)
      })
    })

    connection.onreconnecting(() => setStatus('connecting'))
    connection.onreconnected(() => setStatus('connected'))
    connection.onclose(() => setStatus('disconnected'))

    connection
      .start()
      .then(() => setStatus('connected'))
      .catch((err) => {
        console.error('Error al conectar al Hub:', err)
        setStatus('error')
      })

    return () => {
      connection.stop()
    }
  }, [token])

  const sendMessage = useCallback(async (content) => {
    if (!connectionRef.current || !content.trim()) return
    await connectionRef.current.invoke('SendMessage', content)
  }, [])

  const sendPrivateMessage = useCallback(async (toUsername, content) => {
    if (!connectionRef.current || !content.trim()) return
    await connectionRef.current.invoke('SendPrivateMessage', toUsername, content)
  }, [])

  const notifyTyping = useCallback(async (isTyping) => {
    if (!connectionRef.current) return
    try {
      await connectionRef.current.invoke('Typing', isTyping)
    } catch {
      /* ignoramos errores cosméticos */
    }
  }, [])

  return {
    status,
    messages,
    usersOnline,
    typingUsers: typingUsers.filter((u) => u !== currentUsername),
    sendMessage,
    sendPrivateMessage,
    notifyTyping
  }
}
