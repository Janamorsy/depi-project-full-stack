import { useEffect, useState, useContext } from 'react';
import Navbar from '../Navbar';
import { Loading } from '../ui';
import { AuthContext } from '../../context/AuthContext';
import { hasUnreadMessages } from '../../services/chatService';

const UNREAD_CHECK_INTERVAL = 30000;

const PageLayout = ({ 
  children, 
  loading = false,
  loadingText = 'Loading...',
  containerClass = 'mt-4',
  fluid = false
}) => {
  const { user } = useContext(AuthContext);
  const [hasNewMessages, setHasNewMessages] = useState(false);

  useEffect(() => {
    if (!user?.id) return;

    const checkUnread = async () => {
      const hasUnread = await hasUnreadMessages(user.id);
      setHasNewMessages(hasUnread);
    };

    checkUnread();
    const interval = setInterval(checkUnread, UNREAD_CHECK_INTERVAL);

    return () => clearInterval(interval);
  }, [user?.id]);

  return (
    <>
      <Navbar newMessageFlag={hasNewMessages} />
      <div className={`container${fluid ? '-fluid' : ''} ${containerClass}`}>
        {loading ? (
          <div className="py-5">
            <Loading text={loadingText} />
          </div>
        ) : children}
      </div>
    </>
  );
};

export default PageLayout;
