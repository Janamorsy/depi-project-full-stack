import { useState } from 'react';
import Card from './Card';
import Badge from './Badge';
import Button from './Button';
import Avatar from './Avatar';
import { API_BASE_URL, DEFAULT_HOTEL_IMAGE, getFileUrl } from '../../config/env';

const DEFAULT_AVATAR_FALLBACK = `${API_BASE_URL}/images/default-avatar.png`;

// Simple Image Carousel Component
const ImageCarousel = ({ images, fallbackImage, alt, height = '200px' }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  
  const nextImage = (e) => {
    e.stopPropagation();
    setCurrentIndex((prev) => (prev + 1) % images.length);
  };
  
  const prevImage = (e) => {
    e.stopPropagation();
    setCurrentIndex((prev) => (prev - 1 + images.length) % images.length);
  };
  
  // Get image URL - handle both string and object formats
  const getImageUrl = (img) => {
    const url = typeof img === 'string' ? img : (img?.imageUrl || '');
    return getFileUrl(url) || fallbackImage;
  };
  
  const currentImage = images[currentIndex];
  const imageUrl = getImageUrl(currentImage);
  
  return (
    <div style={{ position: 'relative', width: '100%', height }}>
      <img
        src={imageUrl}
        alt={`${alt} ${currentIndex + 1}`}
        style={{ width: '100%', height: '100%', objectFit: 'cover' }}
        onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = fallbackImage; }}
      />
      {images.length > 1 && (
        <>
          <button
            onClick={prevImage}
            style={{
              position: 'absolute',
              left: '8px',
              top: '50%',
              transform: 'translateY(-50%)',
              backgroundColor: 'rgba(0,0,0,0.5)',
              color: 'white',
              border: 'none',
              borderRadius: '50%',
              width: '32px',
              height: '32px',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              zIndex: 2
            }}
          >
            <i className="bi bi-chevron-left"></i>
          </button>
          <button
            onClick={nextImage}
            style={{
              position: 'absolute',
              right: '8px',
              top: '50%',
              transform: 'translateY(-50%)',
              backgroundColor: 'rgba(0,0,0,0.5)',
              color: 'white',
              border: 'none',
              borderRadius: '50%',
              width: '32px',
              height: '32px',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              zIndex: 2
            }}
          >
            <i className="bi bi-chevron-right"></i>
          </button>
          <div style={{
            position: 'absolute',
            bottom: '8px',
            left: '50%',
            transform: 'translateX(-50%)',
            display: 'flex',
            gap: '6px',
            zIndex: 2
          }}>
            {images.map((_, idx) => (
              <span
                key={idx}
                style={{
                  width: '8px',
                  height: '8px',
                  borderRadius: '50%',
                  backgroundColor: idx === currentIndex ? 'white' : 'rgba(255,255,255,0.5)',
                  cursor: 'pointer',
                  boxShadow: '0 1px 3px rgba(0,0,0,0.3)'
                }}
                onClick={(e) => { e.stopPropagation(); setCurrentIndex(idx); }}
              />
            ))}
          </div>
          <div style={{
            position: 'absolute',
            top: '8px',
            right: '8px',
            backgroundColor: 'rgba(0,0,0,0.6)',
            color: 'white',
            padding: '2px 8px',
            borderRadius: '12px',
            fontSize: '12px',
            zIndex: 2
          }}>
            {currentIndex + 1}/{images.length}
          </div>
        </>
      )}
    </div>
  );
};

const ListingCard = ({
  image,
  images = [], // NEW: array of images for carousel
  imageAlt,
  fallbackImage,
  badges = [],
  title,
  subtitle,
  details = [],
  price,
  priceLabel = '',
  actionLabel = 'Book Now',
  onAction,
  horizontal = false,
  avatarMode = false,
  children
}) => {
  const defaultImage = fallbackImage || (avatarMode ? DEFAULT_AVATAR_FALLBACK : DEFAULT_HOTEL_IMAGE);
  
  // Normalize images - handle both string arrays and object arrays with imageUrl property
  const normalizeImages = (imgs) => {
    if (!imgs || imgs.length === 0) return [];
    return imgs.map(img => {
      if (typeof img === 'string') return img;
      return img?.imageUrl || '';
    }).filter(url => url); // Filter out empty strings
  };
  
  // Use images array if provided, otherwise use single image
  const normalizedImages = normalizeImages(images);
  const imageList = normalizedImages.length > 0 ? normalizedImages : (image ? [image] : []);
  const hasMultipleImages = imageList.length > 1;

  if (horizontal) {
    return (
      <Card className="h-100" noPadding>
        <div className="row g-0">
          <div className={avatarMode ? 'col-md-4' : 'col-md-5'}>
            {avatarMode ? (
              <div className="d-flex align-items-center justify-content-center h-100 p-3">
                <Avatar src={image} alt={imageAlt || title} size="xl" />
              </div>
            ) : imageList.length > 0 ? (
              <ImageCarousel 
                images={imageList} 
                fallbackImage={defaultImage} 
                alt={imageAlt || title}
                height="100%"
              />
            ) : (
              <img
                src={image || defaultImage}
                className="img-fluid rounded-start h-100 w-100"
                alt={imageAlt || title}
                style={{ objectFit: 'cover', minHeight: '200px' }}
                onError={(e) => { e.target.src = defaultImage; }}
              />
            )}
          </div>
          <div className={avatarMode ? 'col-md-8' : 'col-md-7'}>
            <div className="card-body">
              {badges.length > 0 && (
                <div className="mb-2 d-flex flex-wrap gap-1">
                  {badges.map((badge, idx) => (
                    <Badge key={idx} variant={badge.variant || 'primary'}>
                      {badge.icon && <i className={`bi ${badge.icon} me-1`}></i>}
                      {badge.text}
                    </Badge>
                  ))}
                </div>
              )}
              <h5 className="card-title">{title}</h5>
              {subtitle && <p className="card-text text-muted">{subtitle}</p>}
              {details.length > 0 && (
                <p className="card-text">
                  {details.map((detail, idx) => (
                    <span key={idx}>
                      {detail.label && <strong>{detail.label}:</strong>} {detail.value}
                      {idx < details.length - 1 && <br />}
                    </span>
                  ))}
                </p>
              )}
              {children}
              {price !== undefined && (
                <p className="card-text">
                  <strong>${price}</strong> {priceLabel}
                </p>
              )}
              {onAction && (
                <Button fullWidth onClick={onAction}>
                  {actionLabel}
                </Button>
              )}
            </div>
          </div>
        </div>
      </Card>
    );
  }

  return (
    <Card className="h-100" noPadding>
      {avatarMode ? (
        <div className="d-flex justify-content-center pt-4">
          <Avatar src={image} alt={imageAlt || title} size="xl" />
        </div>
      ) : imageList.length > 0 ? (
        <ImageCarousel 
          images={imageList} 
          fallbackImage={defaultImage} 
          alt={imageAlt || title}
          height="200px"
        />
      ) : (
        <img
          src={image || defaultImage}
          className="card-img-top"
          alt={imageAlt || title}
          style={{ height: '200px', objectFit: 'cover' }}
          onError={(e) => { e.target.src = defaultImage; }}
        />
      )}
      <div className="card-body">
        {badges.length > 0 && (
          <div className="mb-2 d-flex flex-wrap gap-1">
            {badges.map((badge, idx) => (
              <Badge key={idx} variant={badge.variant || 'primary'}>
                {badge.icon && <i className={`bi ${badge.icon} me-1`}></i>}
                {badge.text}
              </Badge>
            ))}
          </div>
        )}
        <h5 className="card-title">{title}</h5>
        {subtitle && <p className="card-text text-muted">{subtitle}</p>}
        {details.length > 0 && (
          <p className="card-text">
            {details.map((detail, idx) => (
              <span key={idx}>
                {detail.label && <strong>{detail.label}:</strong>} {detail.value}
                {idx < details.length - 1 && <br />}
              </span>
            ))}
          </p>
        )}
        {children}
        {price !== undefined && (
          <p className="card-text">
            <strong>${price}</strong> {priceLabel}
          </p>
        )}
        {onAction && (
          <Button fullWidth onClick={onAction}>
            {actionLabel}
          </Button>
        )}
      </div>
    </Card>
  );
};

export default ListingCard;
