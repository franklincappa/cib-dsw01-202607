import { useState, useRef } from 'react'
import { useSignalR } from '../hooks/useSignalR'
import UserList from './UserList'
import MessageList from './MessageList'

export default function Chat({ session, onLogout, theme, onToggleTheme }) {
  const { token, username } = session

  const {
    status,
    messages,
    usersOnline,
    typingUsers,
    sendMessage,
    sendPrivateMessage,
    notifyTyping
  } = useSignalR(token, username)

  const [text, setText] = useState('')
  const [selectedUser, setSelectedUser] = useState(null)
  const typingTimer = useRef(null)
  const isTypingRef = useRef(false)

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (!text.trim()) return

    if (selectedUser) {
      await sendPrivateMessage(selectedUser, text)
    } else {
      await sendMessage(text)
    }
    setText('')

    // Forzar fin de "escribiendo..." al enviar
    if (isTypingRef.current) {
      notifyTyping(false)
      isTypingRef.current = false
    }
  }

  const handleChange = (e) => {
    setText(e.target.value)

    if (!isTypingRef.current) {
      notifyTyping(true)
      isTypingRef.current = true
    }
    clearTimeout(typingTimer.current)
    typingTimer.current = setTimeout(() => {
      notifyTyping(false)
      isTypingRef.current = false
    }, 1500)
  }

  const statusLabel = {
    idle:         { text: 'Inicializando…', color: '#9ca3af' },
    connecting:   { text: 'Conectando…',    color: '#f59e0b' },
    connected:    { text: 'Conectado',      color: '#10b981' },
    disconnected: { text: 'Desconectado',   color: '#ef4444' },
    error:        { text: 'Error',          color: '#ef4444' }
  }[status]

  return (
    <div className="chat-shell">
      <header className="chat-topbar">
        <div className="topbar-left">
          <div className="brand">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z" />
            </svg>
            <strong>Chat SignalR</strong>
          </div>
          <span className="status-pill" style={{ color: statusLabel.color }}>
            <span className="dot" style={{ background: statusLabel.color }} />
            {statusLabel.text}
          </span>
        </div>

        <div className="topbar-right">
          <span className="me-pill">👤 {username}</span>
          <button className="theme-toggle" onClick={onToggleTheme}>
            {theme === 'dark' ? '☀' : '☾'}
          </button>
          <button className="btn-secondary" onClick={onLogout}>Salir</button>
        </div>
      </header>

      <div className="chat-body">
        <UserList
          users={usersOnline}
          currentUser={username}
          selectedUser={selectedUser}
          onSelectUser={setSelectedUser}
        />

        <main className="chat-panel">
          <div className="chat-panel-header">
            {selectedUser ? (
              <>
                <h2>Privado con {selectedUser}</h2>
                <small>Solo ustedes dos verán estos mensajes</small>
              </>
            ) : (
              <>
                <h2># Sala general</h2>
                <small>{usersOnline.length} {usersOnline.length === 1 ? 'usuario' : 'usuarios'} en línea</small>
              </>
            )}
          </div>

          <MessageList
            messages={messages}
            currentUser={username}
            selectedUser={selectedUser}
          />

          {typingUsers.length > 0 && !selectedUser && (
            <div className="typing-indicator">
              <span className="typing-dots"><span /><span /><span /></span>
              {typingUsers.length === 1
                ? `${typingUsers[0]} está escribiendo…`
                : `${typingUsers.join(', ')} están escribiendo…`}
            </div>
          )}

          <form className="chat-input" onSubmit={handleSubmit}>
            <input
              type="text"
              placeholder={selectedUser
                ? `Mensaje privado a ${selectedUser}…`
                : 'Escribe un mensaje a la sala…'}
              value={text}
              onChange={handleChange}
              disabled={status !== 'connected'}
              maxLength={500}
            />
            <button type="submit" className="btn-primary" disabled={!text.trim() || status !== 'connected'}>
              Enviar
            </button>
          </form>
        </main>
      </div>
    </div>
  )
}
