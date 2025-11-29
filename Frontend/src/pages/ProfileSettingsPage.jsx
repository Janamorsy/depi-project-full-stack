import { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, FormInput, FormCheckbox, Button, Loading, Avatar, Modal } from '../components/ui';
import { AuthContext } from '../context/AuthContext';
import api from '../services/api';
import { STORAGE_KEYS, MESSAGE_TIMEOUT_MS } from '../config/env';

const ProfileSettingsPage = () => {
  const { user } = useContext(AuthContext);
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    phoneNumber: '',
    profilePicture: '',
    isWheelchairAccessible: false
  });
  const [imagePreview, setImagePreview] = useState('');

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const response = await api.get('/UserProfile');
      const data = response.data;
      setFormData({
        firstName: data.firstName || '',
        lastName: data.lastName || '',
        phoneNumber: data.phoneNumber || '',
        profilePicture: data.profilePicture || '',
        isWheelchairAccessible: data.isWheelchairAccessible || false
      });
      setImagePreview(data.profilePicture || '');
    } catch (err) {
      console.error('Failed to load profile:', err);
      setError('Failed to load profile. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({ ...prev, [name]: type === 'checkbox' ? checked : value }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result;
        setFormData(prev => ({ ...prev, profilePicture: base64String }));
        setImagePreview(base64String);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (!formData.firstName || !formData.lastName || !formData.phoneNumber) {
      setError('First name, last name, and phone number are required.');
      return;
    }

    try {
      setSaving(true);
      await api.put('/UserProfile', formData);
      setSuccess('Profile updated successfully!');
      setTimeout(() => setSuccess(''), MESSAGE_TIMEOUT_MS);
    } catch (err) {
      console.error('Failed to update profile:', err);
      setError(err.response?.data?.message || 'Failed to update profile. Please try again.');
    } finally {
      setSaving(false);
    }
  };

  const handleDeleteAccount = async () => {
    try {
      setDeleting(true);
      await api.delete('/UserProfile');
      localStorage.removeItem(STORAGE_KEYS.TOKEN);
      localStorage.removeItem(STORAGE_KEYS.USER);
      navigate('/login');
    } catch (err) {
      console.error('Failed to delete account:', err);
      setError('Failed to delete account. Please try again.');
      setDeleting(false);
    }
  };

  if (loading) {
    return <PageLayout><Loading fullPage /></PageLayout>;
  }

  return (
    <PageLayout title="Profile Settings">
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-6">
          <Card>
            {error && <div className="alert alert-danger alert-dismissible fade show">{error}<button type="button" className="btn-close" onClick={() => setError('')}></button></div>}
            {success && <div className="alert alert-success alert-dismissible fade show">{success}<button type="button" className="btn-close" onClick={() => setSuccess('')}></button></div>}

            <form onSubmit={handleSubmit}>
              <div className="text-center mb-4">
                <Avatar src={imagePreview} alt="Profile" size="xl" />
                <div className="mt-3">
                  <label htmlFor="profilePictureInput" className="btn btn-outline-primary btn-sm">Change Picture</label>
                  <input type="file" id="profilePictureInput" accept="image/*" onChange={handleImageChange} style={{ display: 'none' }} />
                </div>
              </div>

              <FormInput label="First Name" name="firstName" value={formData.firstName} onChange={handleChange} required />
              <FormInput label="Last Name" name="lastName" value={formData.lastName} onChange={handleChange} required />
              <FormInput label="Phone Number" type="tel" name="phoneNumber" value={formData.phoneNumber} onChange={handleChange} required />
              <FormCheckbox name="isWheelchairAccessible" label="Wheelchair accessible required" checked={formData.isWheelchairAccessible} onChange={handleChange} />

              <div className="d-grid gap-2">
                <Button type="submit" loading={saving}>{saving ? 'Saving...' : 'Save Changes'}</Button>
                <Button variant="outline" onClick={() => navigate('/dashboard')}>Cancel</Button>
              </div>
            </form>

            <hr className="my-4" />

            <div className="danger-zone">
              <h5 className="text-danger mb-3">Danger Zone</h5>
              {!showDeleteConfirm ? (
                <Button variant="danger" onClick={() => setShowDeleteConfirm(true)}>Delete Account</Button>
              ) : (
                <div className="alert alert-warning">
                  <p className="mb-2"><strong>Are you sure?</strong> This action cannot be undone. All your data will be permanently deleted.</p>
                  <div className="d-flex gap-2">
                    <Button variant="danger" onClick={handleDeleteAccount} disabled={deleting}>{deleting ? 'Deleting...' : 'Yes, Delete My Account'}</Button>
                    <Button variant="secondary" onClick={() => setShowDeleteConfirm(false)}>Cancel</Button>
                  </div>
                </div>
              )}
            </div>
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default ProfileSettingsPage;

