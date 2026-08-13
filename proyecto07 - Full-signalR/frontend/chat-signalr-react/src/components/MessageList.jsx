import { useEffect, useRef } from 'react'

function formatTime(iso) {
  const d = new Date(iso)
  return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}

export default function MessageList({ messages, currentUser, selectedUser }) {
  const endRef = useRef(null)

  useEffect(() => {
    endRef.current?.scrollIntoView({ behavior: 'smooth' })
  }, [messages])

  // Filtrado: si hay un usuario seleccionado, mostramos solo los privados con él/ella;
  // si no, mostramos públicos y de sistema.
  const filtered = messages.filter((m) => {
    if (m.type === 'system') return !selectedUser // mostrar sistema solo en sala general
    if (selectedUser) {
      return m.type === 'private' &&
        ((m.user === selectedUser && m.to === currentUser) ||
         (m.user === currentUser && m.to === selectedUser))
    }
    return m.type === 'public'
  })

  return (
    <div className="message-list">
      {filtered.length === 0 && (
        <div className="empty-state">
          <div className="empty-icon">💬</div>
          <p>{selectedUser
            ? `No hay mensajes con ${selectedUser} todavía. Saluda 👋`
            : 'No hay mensajes aún. Sé el primero en escribir.'}</p>
        </div>
      )}

      {filtered.map((m) => {
        if (m.type === 'system') {
          return (
            <div key={m.id} className="message message-system">
              <span>{m.content}</span>
            </div>
          )
        }

        const isMine = m.user === currentUser
        return (
          <div key={m.id} className={`message ${isMine ? 'message-mine' : 'message-other'}`}>
            {!isMine && <div className="message-author">{m.user}</div>}
            <div className="message-bubble">
              {m.type === 'private' && (
                <span className="private-tag">privado</span>
              )}
              <p>{m.content}</p>
              <span className="message-time">{formatTime(m.sentAt)}</span>
            </div>
          </div>
        )
      })}

      <div ref={endRef} />
    </div>
  )
}
