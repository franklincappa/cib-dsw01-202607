import { useState } from 'react'
import { login } from '../api/auth'

export default function Login({ onLogin, theme, onToggleTheme }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const data = await login(username.trim(), password)
      onLogin(data)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  const fillDemo = (u, p) => {
    setUsername(u)
    setPassword(p)
  }

  return (
    <div className="login-container">
      <button
        className="theme-toggle theme-toggle-floating"
        onClick={onToggleTheme}
        aria-label="Cambiar tema"
        title={theme === 'dark' ? 'Modo claro' : 'Modo oscuro'}
      >
        {theme === 'dark' ? '☀' : '☾'}
      </button>

      <div className="login-card">
        <div className="login-header">
          <div className="login-logo">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z" />
            </svg>
          </div>
          <h1>Chat SignalR</h1>
          <p className="login-subtitle">Inicia sesión para entrar al chat en tiempo real</p>
        </div>

        <form onSubmit={handleSubmit} className="login-form">
          <label className="field">
            <span>Usuario</span>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="ej. juan"
              autoComplete="username"
              required
            />
          </label>

          <label className="field">
            <span>Contraseña</span>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••"
              autoComplete="current-password"
              required
            />
          </label>

          {error && <div className="alert-error">{error}</div>}

          <button type="submit" className="btn-primary" disabled={loading}>
            {loading ? 'Conectando…' : 'Entrar'}
          </button>
        </form>

        <div className="demo-users">
          <p>Usuarios de prueba (clic para autocompletar):</p>
          <div className="demo-users-grid">
            <button type="button" onClick={() => fillDemo('admin', 'admin123')}>admin</button>
            <button type="button" onClick={() => fillDemo('juan', 'juan123')}>juan</button>
            <button type="button" onClick={() => fillDemo('maria', 'maria123')}>maria</button>
            <button type="button" onClick={() => fillDemo('carlos', 'carlos123')}>carlos</button>
          </div>
        </div>
      </div>
    </div>
  )
}
