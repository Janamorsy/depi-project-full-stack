import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, FormInput, FormTextarea, FormCheckbox, Button, Loading } from '../components/ui';
import { getMedicalProfile, saveMedicalProfile } from '../services/medicalProfileService';
import { REDIRECT_DELAY_MS } from '../config/env';

const MedicalProfilePage = () => {
  const [profile, setProfile] = useState({
    medicalConditions: '',
    accessibilityNeeds: '',
    treatmentHistory: '',
    consentGiven: false
  });
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const data = await getMedicalProfile();
        setProfile(data);
      } catch (error) {
        if (error.response?.status !== 404) {
          console.error('Failed to fetch profile:', error);
        }
      } finally {
        setLoading(false);
      }
    };
    fetchProfile();
  }, []);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setProfile({ ...profile, [name]: type === 'checkbox' ? checked : value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!profile.consentGiven) {
      setError('You must provide consent to save your medical profile');
      return;
    }

    setError('');
    setSaving(true);

    try {
      await saveMedicalProfile(profile);
      setSuccess('Profile saved successfully!');
      setTimeout(() => navigate('/dashboard'), REDIRECT_DELAY_MS);
    } catch (err) {
      setError('Failed to save profile');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <PageLayout><Loading fullPage /></PageLayout>;
  }

  return (
    <PageLayout>
      <div className="row justify-content-center">
        <div className="col-lg-8">
          <Card title="My Medical Profile">
            <div className="alert alert-info">
              <strong>Privacy Notice:</strong> This information is used solely to personalize your recommendations and is not a substitute for professional medical advice.
            </div>

            {error && <div className="alert alert-danger">{error}</div>}
            {success && <div className="alert alert-success">{success}</div>}

            <form onSubmit={handleSubmit}>
              <FormInput label="Medical Conditions" name="medicalConditions" value={profile.medicalConditions} onChange={handleChange} placeholder="e.g., Atrial Fibrillation" />
              <FormInput label="Accessibility Needs" name="accessibilityNeeds" value={profile.accessibilityNeeds} onChange={handleChange} placeholder="e.g., Wheelchair access required" />
              <FormTextarea label="Treatment History" name="treatmentHistory" value={profile.treatmentHistory} onChange={handleChange} rows={3} placeholder="Brief history of your treatment" />
              <FormCheckbox
                name="consentGiven"
                label="I consent to using this data for personalized recommendations on the platform and understand it is not a substitute for professional medical advice."
                checked={profile.consentGiven}
                onChange={handleChange}
              />
              <Button type="submit" loading={saving}>{saving ? 'Saving...' : 'Save Profile'}</Button>
            </form>
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default MedicalProfilePage;


