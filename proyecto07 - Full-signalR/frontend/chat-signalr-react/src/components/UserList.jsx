function avatarColor(name) {
  // Paleta inspirada en Telegram: azules, cian y teal en distintas intensidades
  const colors = ['#2AABEE', '#1A91D6', '#3390EC', '#5DADE2', '#48C6E0', '#0F8AB8', '#2E86C1', '#1B7BA8']
  let hash = 0
  for (let i = 0; i < name.length; i++) hash = name.charCodeAt(i) + ((hash << 5) - hash)
  return colors[Math.abs(hash) % colors.length]
}

export default function UserList({ users, currentUser, selectedUser, onSelectUser }) {
  return (
    <aside className="user-list">
      <div className="user-list-header">
        <h2>Conectados</h2>
        <span className="badge">{users.length}</span>
      </div>

      <button
        className={`user-item ${!selectedUser ? 'active' : ''}`}
        onClick={() => onSelectUser(null)}
      >
        <div className="avatar avatar-global">#</div>
        <div className="user-info">
          <strong>Sala general</strong>
          <small>Todos los usuarios</small>
        </div>
      </button>

      <div className="user-list-divider">Mensaje privado</div>

      {users.length === 0 && <p className="empty">Nadie conectado…</p>}

      {users.map((u) => {
        const isMe = u.username === currentUser
        const isSelected = selectedUser === u.username
        return (
          <button
            key={u.connectionId}
            className={`user-item ${isSelected ? 'active' : ''}`}
            disabled={isMe}
            onClick={() => onSelectUser(u.username)}
          >
            <div className="avatar" style={{ background: avatarColor(u.username) }}>
              {u.username[0].toUpperCase()}
            </div>
            <div className="user-info">
              <strong>{u.username} {isMe && <span className="me">(tú)</span>}</strong>
              <small><span className="dot dot-online" /> en línea</small>
            </div>
          </button>
        )
      })}
    </aside>
  )
}
