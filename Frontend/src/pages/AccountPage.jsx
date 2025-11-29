import { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, FormInput, FormCheckbox, Button, Avatar } from '../components/ui';
import { AuthContext } from '../context/AuthContext';
import fileUploadService from '../services/fileUploadService';
import axios from 'axios';
import { API_URL } from '../config/env';

const AccountPage = () => {
  const { user, token, login } = useContext(AuthContext);
  const navigate = useNavigate();
  const [editing, setEditing] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState({ type: '', text: '' });
  const [previewImage, setPreviewImage] = useState('');
  const [selectedFile, setSelectedFile] = useState(null);
  
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    profilePicture: '',
    isWheelchairAccessible: false
  });

  useEffect(() => {
    if (user) {
      setFormData({
        firstName: user.firstName || '',
        lastName: user.lastName || '',
        email: user.email || '',
        phoneNumber: user.phoneNumber || '',
        profilePicture: user.profilePicture || '',
        isWheelchairAccessible: user.isWheelchairAccessible || false
      });
      setPreviewImage(fileUploadService.getImageUrl(user.profilePicture));
    }
  }, [user]);

  const handleChange = (e) => {
    const value = e.target.type === 'checkbox' ? e.target.checked : e.target.value;
    setFormData({ ...formData, [e.target.name]: value });
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setSelectedFile(file);
      const reader = new FileReader();
      reader.onloadend = () => setPreviewImage(reader.result);
      reader.readAsDataURL(file);
    }
  };

  const handleImageUpload = async () => {
    if (!selectedFile) return formData.profilePicture;
    setUploading(true);
    try {
      const imageUrl = await fileUploadService.uploadProfileImage(selectedFile);
      return imageUrl;
    } catch (error) {
      console.error('Failed to upload image:', error);
      setMessage({ type: 'error', text: 'Failed to upload profile picture' });
      return formData.profilePicture;
    } finally {
      setUploading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    setMessage({ type: '', text: '' });

    try {
      let profilePictureUrl = formData.profilePicture;
      if (selectedFile) {
        profilePictureUrl = await handleImageUpload();
      }

      const updateData = { ...formData, profilePicture: profilePictureUrl };
      const response = await axios.put(`${API_URL}/Auth/profile`, updateData, {
        headers: { Authorization: `Bearer ${token}` }
      });

      login({ ...response.data, token });
      setMessage({ type: 'success', text: 'Profile updated successfully!' });
      setEditing(false);
      setSelectedFile(null);
    } catch (error) {
      console.error('Failed to update profile:', error);
      setMessage({ type: 'error', text: error.response?.data?.message || 'Failed to update profile' });
    } finally {
      setSaving(false);
    }
  };

  const handleCancel = () => {
    setEditing(false);
    setSelectedFile(null);
    setPreviewImage(fileUploadService.getImageUrl(user.profilePicture));
    setFormData({
      firstName: user.firstName || '',
      lastName: user.lastName || '',
      email: user.email || '',
      phoneNumber: user.phoneNumber || '',
      profilePicture: user.profilePicture || '',
      isWheelchairAccessible: user.isWheelchairAccessible || false
    });
  };

  if (!user) {
    navigate('/login');
    return null;
  }

  return (
    <PageLayout title="Account Settings">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <Card
            headerAction={!editing && <Button onClick={() => setEditing(true)}><i className="fas fa-edit me-2"></i>Edit Profile</Button>}
          >
            {message.text && (
              <div className={`alert alert-${message.type === 'success' ? 'success' : 'danger'}`}>{message.text}</div>
            )}

            <form onSubmit={handleSubmit}>
              <div className="text-center mb-4">
                <Avatar src={previewImage} alt="Profile" size="xl" />
                {editing && (
                  <div className="mt-3">
                    <input type="file" className="form-control" accept="image/*" onChange={handleFileChange} />
                    <small className="text-muted">Upload a new profile picture</small>
                  </div>
                )}
              </div>

              <div className="row mb-3">
                <div className="col-md-6">
                  <FormInput label="First Name" name="firstName" value={formData.firstName} onChange={handleChange} disabled={!editing} required />
                </div>
                <div className="col-md-6">
                  <FormInput label="Last Name" name="lastName" value={formData.lastName} onChange={handleChange} disabled={!editing} required />
                </div>
              </div>

              <FormInput label="Email" type="email" name="email" value={formData.email} onChange={handleChange} disabled helpText="Email cannot be changed" />
              <FormInput label="Phone Number" type="tel" name="phoneNumber" value={formData.phoneNumber} onChange={handleChange} disabled={!editing} />
              <FormCheckbox name="isWheelchairAccessible" label="I require wheelchair accessible facilities" checked={formData.isWheelchairAccessible} onChange={handleChange} disabled={!editing} />

              {editing && (
                <div className="d-flex gap-2">
                  <Button type="submit" loading={saving || uploading}>{saving || uploading ? 'Saving...' : 'Save Changes'}</Button>
                  <Button variant="outline" onClick={handleCancel} disabled={saving || uploading}>Cancel</Button>
                </div>
              )}
            </form>
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default AccountPage;

