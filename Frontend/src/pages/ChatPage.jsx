import { useState, useEffect, useContext, useRef } from 'react';
import { useParams, useLocation, useNavigate } from 'react-router-dom';
import { PageLayout, DoctorPageLayout } from '../components/layouts';
import { Card, Loading, EmptyState, Button, Avatar } from '../components/ui';
import { getConversation, sendMessage, markConversationAsRead } from '../services/chatService';
import { DoctorAuthContext } from '../context/DoctorAuthContext';
import { AuthContext } from '../context/AuthContext';
import { API_BASE_URL, CHAT_POLL_INTERVAL_MS } from '../config/env';

const ChatPage = () => {
  const { userId: paramUserId } = useParams();
  const location = useLocation();
  const navigate = useNavigate();
  const { doctor } = useContext(DoctorAuthContext);
  const { user } = useContext(AuthContext);
  const currentUser = user || doctor;
  const isDoctor = !!doctor;
  const messagesEndRef = useRef(null);
  
  const userId = paramUserId || location.state?.doctorId || location.state?.patientId;
  const [userName, setUserName] = useState(location.state?.doctorName || location.state?.patientName || '');
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const [selectedFile, setSelectedFile] = useState(null); 
  const [fileError, setFileError] = useState('');
  const [loading, setLoading] = useState(true);
  const lastMessageIdRef = useRef(0);
  const isSendingRef = useRef(false);

  const getLastRealMessageId = (currentMessages) => {
    const realMessages = currentMessages.filter(m => m.id > 0);
    return realMessages.length ? Math.max(...realMessages.map(m => m.id)) : 0;
  };

  const sortMessages = (msgs) => {
    return msgs.sort((a, b) => {
      const dateA = Date.parse(a.createdAt);
      const dateB = Date.parse(b.createdAt);
      if (isNaN(dateA) && isNaN(dateB)) return 0;
      if (isNaN(dateA)) return 1; 
      if (isNaN(dateB)) return -1;
      return dateA - dateB; 
    });
  };

  const scrollToBottom = () => {
    setTimeout(() => messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' }), 50);
  };

  useEffect(() => {
    if (!userId) {
      setLoading(false);
      return;
    }

    let isActive = true;
    let intervalId = null;

    const fetchNewMessages = async () => {
      if (!isActive) return;
      try {
        const lastId = lastMessageIdRef.current;
        const newMsgs = await getConversation(userId, lastId);
        if (isActive && newMsgs.length > 0) {
          setMessages(prev => {
            const realMessages = prev.filter(m => m.id > 0);
            return sortMessages([...realMessages, ...newMsgs]);
          });
          lastMessageIdRef.current = Math.max(lastId, ...newMsgs.map(m => m.id));
          scrollToBottom();
          // Mark new messages as read
          markConversationAsRead(userId).catch(console.error);
        }
      } catch (error) {
        console.error('Fetch error:', error);
      }
    };

    const loadInitial = async () => {
      try {
        const data = await getConversation(userId, 0);
        if (isActive) {
          setMessages(sortMessages(data));
          if (data.length > 0) {
            lastMessageIdRef.current = getLastRealMessageId(data);
            // Mark conversation as read when opened
            markConversationAsRead(userId).catch(console.error);
            // Extract other user's name from messages if not provided via state
            if (!location.state?.doctorName && !location.state?.patientName) {
              const firstMsg = data[0];
              if (isDoctor && firstMsg.patientName) {
                setUserName(firstMsg.patientName);
              } else if (!isDoctor && firstMsg.doctorName) {
                setUserName(firstMsg.doctorName);
              }
            }
          }
          setLoading(false);
          scrollToBottom();
        }
      } catch (error) {
        console.error('Load error:', error);
        if (isActive) setLoading(false);
      }
    };

    loadInitial();
    intervalId = setInterval(fetchNewMessages, CHAT_POLL_INTERVAL_MS);
    
    return () => {
      isActive = false;
      if (intervalId) clearInterval(intervalId);
    };
  }, [userId]);

  const handleSendMessage = async (e) => {
    e.preventDefault();
    if ((!newMessage.trim() && !selectedFile) || !userId || isSendingRef.current) return;

    // Require text when sending a file
    if (selectedFile && !newMessage.trim()) {
      setFileError('Please add a message to send with your file');
      return;
    }

    setFileError('');
    const messageText = newMessage.trim();
    const fileToSend = selectedFile;
    const tempId = -Date.now();
    const tempFileUrl = fileToSend ? URL.createObjectURL(fileToSend) : null;

    const tempMsg = {
      id: tempId,
      message: messageText,
      fileUrl: tempFileUrl,
      senderType: isDoctor ? 'Doctor' : 'Patient',
      createdAt: new Date().toISOString(),
      patientId: isDoctor ? userId : currentUser.id,
      doctorId: isDoctor ? currentUser.id : userId,
      patientName: !isDoctor ? `${currentUser.firstName} ${currentUser.lastName}` : userName,
      doctorName: isDoctor ? `Dr. ${currentUser.firstName} ${currentUser.lastName}` : userName,
      patientProfilePicture: !isDoctor ? currentUser.profilePicture : null,
      doctorProfilePicture: isDoctor ? currentUser.imageUrl : null
    };

    setMessages(prev => [...prev, tempMsg]);
    setNewMessage('');
    setSelectedFile(null);
    scrollToBottom();
    isSendingRef.current = true;

    try {
      await sendMessage({ recipientId: userId, message: messageText || '', file: fileToSend || null });
    } catch (error) {
      console.error("Send error:", error);
      setMessages(prev => prev.filter(m => m.id !== tempId));
      if (messageText) setNewMessage(messageText);
      setSelectedFile(fileToSend);
    } finally {
      isSendingRef.current = false;
    }
  };

  const handleFileUpload = (e) => {
    const file = e.target.files[0];
    if (!file) return;
    setSelectedFile(file);
    setFileError('');
    e.target.value = null;
  };

  const handleMessageChange = (e) => {
    setNewMessage(e.target.value);
    if (fileError) setFileError('');
  };

  const isImageFile = (url) => /\.(jpg|jpeg|png|gif|bmp|webp)$/i.test(url);
  const getFileUrl = (url) => url.startsWith('blob:') ? url : `${API_BASE_URL}${url}`;

  const renderFileAttachment = (fileUrl) => {
    if (!fileUrl) return null;
    
    return (
      <div className="file-preview mt-2">
        {isImageFile(fileUrl) ? (
          <img src={getFileUrl(fileUrl)} alt="attachment" style={{ maxWidth: '200px', borderRadius: '5px', display: 'block' }} />
        ) : (
          <a href={getFileUrl(fileUrl)} target="_blank" rel="noopener noreferrer" className="btn btn-outline-secondary btn-sm">
            <i className="bi bi-download me-1"></i>Download File
          </a>
        )}
      </div>
    );
  };

  const renderMessage = (msg, idx) => {
    const isMine = msg.senderType === (isDoctor ? 'Doctor' : 'Patient');
    const profilePic = msg.senderType === 'Doctor' ? msg.doctorProfilePicture : msg.patientProfilePicture;
    const senderName = msg.senderType === 'Patient' ? msg.patientName : msg.doctorName;

    return (
      <div key={msg.id || idx} className={`mb-3 d-flex ${isMine ? 'justify-content-end' : 'justify-content-start'}`}>
        {!isMine && <Avatar src={profilePic} alt={msg.senderType} size="sm" className="me-2" />}
        <div className={`d-inline-block p-3 rounded-3 ${isMine ? 'bg-primary text-white' : 'bg-light'}`} style={{ maxWidth: '70%' }}>
          <small className="fw-bold d-block mb-1">{senderName}</small>
          {msg.message && <p className="mb-1">{msg.message}</p>}
          {renderFileAttachment(msg.fileUrl)}
          <small className={`d-block mt-2 ${isMine ? 'text-white-50' : 'text-muted'}`}>
            {new Date(msg.createdAt).toLocaleTimeString()}
            {msg.id < 0 && <span className="text-warning ms-2"><i className="bi bi-clock"></i> Sending...</span>}
          </small>
        </div>
        {isMine && <Avatar src={profilePic} alt={msg.senderType} size="sm" className="ms-2" />}
      </div>
    );
  };

  const renderChatContent = () => {
    if (!userId) {
      return <EmptyState icon="bi-chat-dots" title="No conversation selected" description="Please select a doctor or patient to start chatting." />;
    }

    return (
      <Card>
        <div className="card-header bg-primary text-white d-flex align-items-center">
          <i className="bi bi-chat-dots me-2"></i>
          <h5 className="mb-0">Chat with {userName || 'User'}</h5>
        </div>
        <div className="card-body" style={{ height: '500px', overflowY: 'auto' }}>
          {loading ? (
            <Loading text="Loading messages..." />
          ) : messages.length === 0 ? (
            <EmptyState icon="bi-chat-text" title="No messages yet" description="Start the conversation by sending a message!" size="sm" />
          ) : (
            <>
              {messages.map(renderMessage)}
              <div ref={messagesEndRef} />
            </>
          )}
        </div>
        <div className="card-footer bg-light">
          {fileError && (
            <div className="alert alert-warning py-2 mb-2 d-flex align-items-center" style={{ fontSize: '14px' }}>
              <span className="me-2">⚠️</span>
              {fileError}
            </div>
          )}
          <form onSubmit={handleSendMessage}>
            <div className="d-flex align-items-center gap-2">
              <input type="file" id="fileUpload" style={{ display: "none" }} onChange={handleFileUpload} />
              <Button type="button" variant="outline" onClick={() => document.getElementById("fileUpload").click()} disabled={isSendingRef.current} title="Attach file">
                <i className="bi bi-paperclip"></i>
              </Button>
              {selectedFile && (
                <span className="badge bg-secondary d-flex align-items-center">
                  <i className="bi bi-file-earmark me-1"></i>
                  {selectedFile.name.length > 15 ? selectedFile.name.substring(0, 15) + '...' : selectedFile.name}
                  <button type="button" className="btn-close btn-close-white ms-2" style={{ fontSize: '0.6rem' }} onClick={() => { setSelectedFile(null); setFileError(''); }}></button>
                </span>
              )}
              <input type="text" className="form-control" placeholder={selectedFile ? "Add a message for your file..." : "Type your message..."} value={newMessage} onChange={handleMessageChange} disabled={isSendingRef.current} />
              <Button type="submit" disabled={(!newMessage.trim() && !selectedFile) || isSendingRef.current}>
                {isSendingRef.current ? <><i className="bi bi-hourglass-split"></i> Sending</> : <><i className="bi bi-send"></i> Send</>}
              </Button>
            </div>
          </form>
        </div>
      </Card>
    );
  };

  const Layout = isDoctor ? DoctorPageLayout : PageLayout;
  const layoutProps = isDoctor ? {
    headerAction: (
      <Button variant="outline" size="sm" onClick={() => navigate('/doctor/dashboard')}>
        <i className="bi bi-arrow-left me-1"></i>Back to Dashboard
      </Button>
    )
  } : {};

  return (
    <Layout {...layoutProps}>
      <div className="row justify-content-center">
        <div className="col-lg-8">
          {renderChatContent()}
        </div>
      </div>
    </Layout>
  );
};

export default ChatPage;
