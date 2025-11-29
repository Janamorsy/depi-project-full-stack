import { useEffect, useState, useContext } from 'react';
import { Link } from 'react-router-dom';
import { DoctorPageLayout } from '../../components/layouts';
import { Card, Loading, EmptyState, Badge, Avatar } from '../../components/ui';
import { DoctorAuthContext } from '../../context/DoctorAuthContext';
import { getAllChats } from '../../services/chatService';

const DoctorInboxPage = () => {
  const { doctor } = useContext(DoctorAuthContext);
  const [chats, setChats] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!doctor) return;

    const fetchChats = async () => {
      try {
        const data = await getAllChats(doctor.id);
        setChats(data);
      } catch (err) {
        console.error('Failed to fetch chats', err);
      } finally {
        setLoading(false);
      }
    };

    fetchChats();
  }, [doctor]);

  const getOtherUser = (chat) => {
    if (!chat.patientId || !chat.doctorId) return null;
    // Doctor is viewing, so other user is the patient
    return chat.patient;
  };

  const getUserName = (otherUser) => {
    if (!otherUser) return 'Unknown';
    return otherUser.firstName && otherUser.lastName
      ? `${otherUser.firstName} ${otherUser.lastName}`
      : 'Unknown';
  };

  const renderChatItem = (chat) => {
    const otherUser = getOtherUser(chat);
    if (!otherUser) return null;

    const otherUserName = getUserName(otherUser);
    const otherUserId = otherUser?.id;
    const isUnread = !chat.isRead;

    return (
      <Link
        key={chat.id}
        to={`/doctor/chat/${otherUserId}`}
        state={{ patientId: otherUserId, patientName: otherUserName }}
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
          description="Conversations with your patients will appear here."
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
    <DoctorPageLayout>
      <div className="row justify-content-center">
        <div className="col-lg-8">
          <Card>
            <div className="card-header bg-success text-white d-flex align-items-center">
              <i className="bi bi-inbox me-2"></i>
              <h5 className="mb-0">Patient Messages</h5>
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
    </DoctorPageLayout>
  );
};

export default DoctorInboxPage;
