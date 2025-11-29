import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { DoctorPageLayout } from '../../components/layouts';
import { Card, Button, Loading, Avatar, FormInput, FormTextarea, Modal, AvailabilityScheduler } from '../../components/ui';
import api from '../../services/api';
import { STORAGE_KEYS, MESSAGE_TIMEOUT_MS } from '../../config/env';
import { useToast } from '../../context/ToastContext';

const INITIAL_FORM_STATE = {
  firstName: '',
  lastName: '',
  phoneNumber: '',
  imageUrl: '',
  specialty: '',
  hospital: '',
  bio: '',
  consultationFee: 0,
  languages: '',
  specialtyTags: '',
  education: '',
  experience: ''
};

const REQUIRED_FIELDS = [
  'firstName', 'lastName', 'phoneNumber', 'specialty', 
  'hospital', 'bio', 'languages', 'specialtyTags'
];

function DoctorProfileSettingsPage() {
  const navigate = useNavigate();
  const { showSuccess, showError } = useToast();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [savingAvailability, setSavingAvailability] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [formData, setFormData] = useState(INITIAL_FORM_STATE);
  const [imagePreview, setImagePreview] = useState('');
  const [availabilitySlots, setAvailabilitySlots] = useState([]);

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const response = await api.get('/doctor/profile', {
        headers: { Authorization: `Bearer ${localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN)}` }
      });
      const data = response.data;
      setFormData({
        firstName: data.firstName || '',
        lastName: data.lastName || '',
        phoneNumber: data.phoneNumber || '',
        imageUrl: data.imageUrl || '',
        specialty: data.specialty || '',
        hospital: data.hospital || '',
        bio: data.bio || '',
        consultationFee: data.consultationFee || 0,
        languages: data.languages || '',
        specialtyTags: data.specialtyTags || '',
        education: data.education || '',
        experience: data.experience || ''
      });
      setImagePreview(data.imageUrl || '');
      setAvailabilitySlots(data.availabilitySlots || []);
    } catch (err) {
      console.error('Failed to load profile:', err);
      setError('Failed to load profile. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'number' ? parseFloat(value) : value
    }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result;
        setFormData(prev => ({ ...prev, imageUrl: base64String }));
        setImagePreview(base64String);
      };
      reader.readAsDataURL(file);
    }
  };

  const validateForm = () => {
    for (const field of REQUIRED_FIELDS) {
      if (!formData[field]) {
        const fieldName = field.replace(/([A-Z])/g, ' $1').trim();
        setError(`${fieldName} is required.`);
        return false;
      }
    }
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (!validateForm()) return;

    try {
      setSaving(true);
      await api.put('/doctor/profile', formData, {
        headers: { Authorization: `Bearer ${localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN)}` }
      });
      setSuccess('Profile updated successfully!');
      setTimeout(() => setSuccess(''), MESSAGE_TIMEOUT_MS);
    } catch (err) {
      console.error('Failed to update profile:', err);
      setError(err.response?.data?.message || 'Failed to update profile. Please try again.');
    } finally {
      setSaving(false);
    }
  };

  const handleSaveAvailability = async () => {
    try {
      setSavingAvailability(true);
      await api.put('/doctor/profile/availability', 
        { slots: availabilitySlots },
        { headers: { Authorization: `Bearer ${localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN)}` } }
      );
      showSuccess('Availability updated successfully!');
    } catch (err) {
      console.error('Failed to update availability:', err);
      showError('Failed to update availability. Please try again.');
    } finally {
      setSavingAvailability(false);
    }
  };

  const handleDeleteAccount = async () => {
    try {
      setDeleting(true);
      await api.delete('/doctor/profile', {
        headers: { Authorization: `Bearer ${localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN)}` }
      });
      localStorage.removeItem(STORAGE_KEYS.DOCTOR_TOKEN);
      localStorage.removeItem(STORAGE_KEYS.DOCTOR);
      navigate('/doctor/login');
    } catch (err) {
      console.error('Failed to delete account:', err);
      setError('Failed to delete account. Please try again.');
      setDeleting(false);
      setShowDeleteModal(false);
    }
  };

  if (loading) {
    return (
      <DoctorPageLayout>
        <Loading text="Loading profile..." />
      </DoctorPageLayout>
    );
  }

  return (
    <DoctorPageLayout
      title="Profile Settings"
      subtitle="Manage your professional profile"
      headerAction={
        <Button variant="outline" size="sm" onClick={() => navigate('/doctor/dashboard')}>
          <i className="bi bi-arrow-left me-1"></i>Back to Dashboard
        </Button>
      }
    >
      <div className="row justify-content-center">
        <div className="col-md-10 col-lg-8">
          <Card>
            {error && (
              <div className="alert alert-danger alert-dismissible fade show" role="alert">
                {error}
                <button type="button" className="btn-close" onClick={() => setError('')}></button>
              </div>
            )}
            {success && (
              <div className="alert alert-success alert-dismissible fade show" role="alert">
                {success}
                <button type="button" className="btn-close" onClick={() => setSuccess('')}></button>
              </div>
            )}

            <form onSubmit={handleSubmit}>
              {/* Profile Picture */}
              <div className="text-center mb-4">
                <Avatar src={imagePreview} alt="Profile" size="xl" />
                <div className="mt-3">
                  <label htmlFor="profilePictureInput" className="btn btn-outline-success btn-sm">
                    <i className="bi bi-camera me-1"></i>Change Picture
                  </label>
                  <input type="file" id="profilePictureInput" accept="image/*" onChange={handleImageChange} style={{ display: 'none' }} />
                </div>
              </div>

              {/* Basic Information */}
              <h6 className="text-muted mb-3"><i className="bi bi-person me-2"></i>Basic Information</h6>
              <div className="row">
                <div className="col-md-6">
                  <FormInput label="First Name" name="firstName" value={formData.firstName} onChange={handleChange} required />
                </div>
                <div className="col-md-6">
                  <FormInput label="Last Name" name="lastName" value={formData.lastName} onChange={handleChange} required />
                </div>
              </div>

              <FormInput label="Phone Number" name="phoneNumber" type="tel" value={formData.phoneNumber} onChange={handleChange} required />

              {/* Professional Details */}
              <h6 className="text-muted mb-3 mt-4"><i className="bi bi-briefcase me-2"></i>Professional Details</h6>
              <div className="row">
                <div className="col-md-6">
                  <FormInput label="Specialty" name="specialty" value={formData.specialty} onChange={handleChange} required />
                </div>
                <div className="col-md-6">
                  <FormInput label="Hospital" name="hospital" value={formData.hospital} onChange={handleChange} required />
                </div>
              </div>

              <FormTextarea label="Bio" name="bio" value={formData.bio} onChange={handleChange} rows={4} required />

              <div className="row">
                <div className="col-md-6">
                  <FormInput label="Consultation Fee ($)" name="consultationFee" type="number" value={formData.consultationFee} onChange={handleChange} min="0" step="0.01" />
                </div>
                <div className="col-md-6">
                  <FormInput label="Languages" name="languages" value={formData.languages} onChange={handleChange} placeholder="e.g., English, Arabic, French" required />
                </div>
              </div>

              <FormInput label="Specialty Tags" name="specialtyTags" value={formData.specialtyTags} onChange={handleChange} placeholder="e.g., Cardiology, Heart Surgery, Preventive Care" required />

              {/* Education & Experience */}
              <h6 className="text-muted mb-3 mt-4"><i className="bi bi-mortarboard me-2"></i>Education & Experience</h6>
              <FormTextarea label="Education" name="education" value={formData.education} onChange={handleChange} rows={2} placeholder="Medical degrees, certifications" />
              <FormTextarea label="Experience" name="experience" value={formData.experience} onChange={handleChange} rows={2} placeholder="Years of practice, previous positions" />

              {/* Actions */}
              <div className="d-grid gap-2 mt-4">
                <Button type="submit" loading={saving}>
                  {saving ? 'Saving...' : <><i className="bi bi-check-lg me-2"></i>Save Profile</>}
                </Button>
              </div>
            </form>

            {/* Availability Section - Separate from profile form */}
            <hr className="my-4" />
            <h6 className="text-muted mb-3"><i className="bi bi-clock me-2"></i>Availability Schedule</h6>
            <AvailabilityScheduler 
              slots={availabilitySlots} 
              onChange={setAvailabilitySlots}
              saving={savingAvailability}
            />
            <div className="d-grid mt-3">
              <Button onClick={handleSaveAvailability} loading={savingAvailability} variant="success">
                {savingAvailability ? 'Saving...' : <><i className="bi bi-calendar-check me-2"></i>Save Availability</>}
              </Button>
            </div>

            {/* Danger Zone */}
            <hr className="my-4" />
            <div className="danger-zone">
              <h6 className="text-danger mb-3"><i className="bi bi-exclamation-triangle me-2"></i>Danger Zone</h6>
              <Button variant="outline-danger" onClick={() => setShowDeleteModal(true)}>
                <i className="bi bi-trash me-2"></i>Delete Account
              </Button>
            </div>
          </Card>
        </div>
      </div>

      {/* Delete Confirmation Modal */}
      <Modal show={showDeleteModal} onClose={() => setShowDeleteModal(false)} title="Delete Account" size="md">
        <div className="alert alert-danger mb-3">
          <i className="bi bi-exclamation-triangle me-2"></i>
          <strong>Warning:</strong> This action cannot be undone.
        </div>
        <p>All your data including appointments and patient records will be permanently deleted.</p>
        <div className="d-flex gap-2 justify-content-end mt-4">
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>Cancel</Button>
          <Button variant="danger" onClick={handleDeleteAccount} loading={deleting}>
            {deleting ? 'Deleting...' : 'Yes, Delete My Account'}
          </Button>
        </div>
      </Modal>
    </DoctorPageLayout>
  );
}

export default DoctorProfileSettingsPage;

