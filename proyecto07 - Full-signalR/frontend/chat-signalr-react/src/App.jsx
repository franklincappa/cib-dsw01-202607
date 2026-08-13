import { useEffect, useState } from 'react'
import Login from './components/Login'
import Chat from './components/Chat'

const SESSION_KEY = 'chat-signalr-session'
const THEME_KEY = 'chat-signalr-theme'

export default function App() {
  const [session, setSession] = useState(() => {
    try {
      const raw = localStorage.getItem(SESSION_KEY)
      return raw ? JSON.parse(raw) : null
    } catch { return null }
  })

  const [theme, setTheme] = useState(() => {
    return localStorage.getItem(THEME_KEY) || 'dark'
  })

  useEffect(() => {
    document.documentElement.dataset.theme = theme
    localStorage.setItem(THEME_KEY, theme)
  }, [theme])

  // Si el JWT expiró, cerramos sesión.
  useEffect(() => {
    if (!session) return
    const exp = new Date(session.expiresAt).getTime()
    if (exp < Date.now()) {
      handleLogout()
      return
    }
    const t = setTimeout(handleLogout, exp - Date.now())
    return () => clearTimeout(t)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [session])

  const handleLogin = (data) => {
    setSession(data)
    localStorage.setItem(SESSION_KEY, JSON.stringify(data))
  }

  const handleLogout = () => {
    setSession(null)
    localStorage.removeItem(SESSION_KEY)
  }

  const toggleTheme = () => setTheme((t) => (t === 'dark' ? 'light' : 'dark'))

  if (!session) {
    return <Login onLogin={handleLogin} theme={theme} onToggleTheme={toggleTheme} />
  }

  return (
    <Chat
      session={session}
      onLogout={handleLogout}
      theme={theme}
      onToggleTheme={toggleTheme}
    />
  )
}
