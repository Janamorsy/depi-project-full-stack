import fileUploadService from '../../services/fileUploadService';
import { API_BASE_URL } from '../../config/env';

const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';

const Avatar = ({ 
  src, 
  alt = 'Avatar', 
  size = 'md',
  className = '' 
}) => {
  const sizeStyles = {
    xs: { width: '24px', height: '24px' },
    sm: { width: '32px', height: '32px' },
    md: { width: '48px', height: '48px' },
    lg: { width: '60px', height: '60px' },
    xl: { width: '120px', height: '120px' }
  };

  const imageUrl = fileUploadService.getImageUrl(src);
  const fallbackUrl = `${API_BASE_URL}${DEFAULT_AVATAR_PATH}`;

  return (
    <img
      src={imageUrl}
      alt={alt}
      className={`rounded-circle ${className}`}
      style={{ 
        ...sizeStyles[size], 
        objectFit: 'cover' 
      }}
      onError={(e) => { e.target.src = fallbackUrl; }}
    />
  );
};

export default Avatar;
