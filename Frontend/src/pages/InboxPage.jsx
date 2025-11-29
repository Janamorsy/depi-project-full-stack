import { useEffect, useState, useContext } from 'react';
import { Link } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, Loading, EmptyState, Badge, Avatar } from '../components/ui';
import { AuthContext } from '../context/AuthContext';
import { getAllChats } from '../services/chatService';

const InboxPage = () => {
  const { user } = useContext(AuthContext);
  const [chats, setChats] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!user) return;

    const fetchChats = async () => {
      try {
        const data = await getAllChats(user.id);
        setChats(data);
      } catch (err) {
        console.error('Failed to fetch chats', err);
      } finally {
        setLoading(false);
      }
    };

    fetchChats();
  }, [user]);

  const getOtherUser = (chat) => {
    if (!chat.patientId || !chat.doctorId) return null;
    return chat.patientId === user.id ? chat.doctor : chat.patient;
  };

  const getUserName = (otherUser) => {
    if (!otherUser) return 'Unknown';
    return otherUser.firstName && otherUser.lastName
      ? `${otherUser.firstName} ${otherUser.lastName}`
      : 'Unknown';
  };

  const getUserId = (otherUser) => {
    return otherUser?.id || otherUser?.patientId || otherUser?.doctorId;
  };

  const renderChatItem = (chat) => {
    const otherUser = getOtherUser(chat);
    if (!otherUser) return null;

    const otherUserName = getUserName(otherUser);
    const otherUserId = getUserId(otherUser);
    const isUnread = !chat.isRead;
    const isOtherUserDoctor = chat.patientId === user.id;

    return (
      <Link
        key={chat.id}
        to={`/chat/${otherUserId}`}
        state={isOtherUserDoctor ? { doctorId: otherUserId, doctorName: otherUserName } : { patientId: otherUserId, patientName: otherUserName }}
        className="list-group-item list-group-item-action d-flex justify-content-between align-items-center"
      >
        <div className="d-flex align-items-center gap-3">
          <Avatar src={otherUser.profilePicture} alt={otherUserName} size="md" />
          <div>
            <h6 className="mb-0">
              {otherUserName}
              {isUnread && <Badge variant="danger" className="ms-2">New</Badge>}
            </h6>
            <small className="text-muted">
              {chat.message?.substring(0, 50)}
              {chat.message?.length > 50 && '...'}
            </small>
          </div>
        </div>
        <small className="text-muted">
          {new Date(chat.createdAt).toLocaleString()}
        </small>
      </Link>
    );
  };

  const renderContent = () => {
    if (loading) {
      return <Loading text="Loading conversations..." />;
    }

    if (chats.length === 0) {
      return (
        <EmptyState
          icon="bi-chat-dots"
          title="No conversations yet"
          description="Start a conversation with a doctor to see your messages here."
        />
      );
    }

    return (
      <div className="list-group list-group-flush">
        {chats.map(renderChatItem)}
      </div>
    );
  };

  return (
    <PageLayout title="Inbox" subtitle="View and manage your conversations">
      <div className="row justify-content-center">
        <div className="col-lg-8">
          <Card>
            <div className="card-header bg-primary text-white d-flex align-items-center">
              <i className="bi bi-inbox me-2"></i>
              <h5 className="mb-0">Your Messages</h5>
              {chats.length > 0 && (
                <Badge variant="light" className="ms-auto">{chats.length} conversations</Badge>
              )}
            </div>
            <div className="card-body p-0">
              {renderContent()}
            </div>
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default InboxPage;
