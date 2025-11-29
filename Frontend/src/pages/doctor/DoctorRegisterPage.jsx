import { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { DoctorAuthContext } from '../../context/DoctorAuthContext';
import { doctorRegister } from '../../services/doctorAuthService';
import fileUploadService from '../../services/fileUploadService';

const DoctorRegisterPage = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    specialty: '',
    specialtyTags: '',
    hospital: '',
    city: '',
    languages: '',
    consultationFee: 0,
    yearsOfExperience: 0,
    bio: '',
    imageUrl: ''
  });
  
  // Profile Picture State
  const [previewImage, setPreviewImage] = useState('https://avatar.iran.liara.run/public/10');
  const [selectedFile, setSelectedFile] = useState(null);

  // National ID State
  const [idFrontFile, setIdFrontFile] = useState(null);
  const [idBackFile, setIdBackFile] = useState(null);

  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useContext(DoctorAuthContext);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // Handler for Profile Picture
  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setSelectedFile(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewImage(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  // Handlers for National IDs
  const handleIdFrontChange = (e) => {
    if (e.target.files[0]) {
      setIdFrontFile(e.target.files[0]);
    }
  };

  const handleIdBackChange = (e) => {
    if (e.target.files[0]) {
      setIdBackFile(e.target.files[0]);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      let imageUrl = '';
      let nationalIdFrontImageUrl = '';
      let nationalIdBackImageUrl = '';

      // 1. Upload Profile Picture
      if (selectedFile) {
        imageUrl = await fileUploadService.uploadDoctorImage(selectedFile);
      }

      // 2. Upload National ID Front
      if (idFrontFile) {
        nationalIdFrontImageUrl = await fileUploadService.uploadDoctorImage(idFrontFile);
      }

      // 3. Upload National ID Back
      if (idBackFile) {
        nationalIdBackImageUrl = await fileUploadService.uploadDoctorImage(idBackFile);
      }

      const registrationData = {
        ...formData,
        imageUrl: imageUrl,
        nationalIdFrontImageUrl: nationalIdFrontImageUrl,
        nationalIdBackImageUrl: nationalIdBackImageUrl
      };

      const doctorData = await doctorRegister(registrationData);
      login(doctorData);
      navigate('/doctor/dashboard');
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || 'Registration failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-vh-100 d-flex align-items-center bg-light py-5">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8">
            <div className="card shadow">
              <div className="card-body p-5">
                <h2 className="text-center mb-4">Doctor Registration</h2>
                <p className="text-center text-muted mb-4">Join NileCare as a medical professional</p>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit}>
                  {/* --- Profile Picture Section --- */}
                  <div className="mb-3 text-center">
                    <img 
                      src={previewImage} 
                      alt="Profile Preview" 
                      className="rounded-circle mb-2"
                      style={{ width: '120px', height: '120px', objectFit: 'cover' }}
                      onError={(e) => { e.target.src = 'https://avatar.iran.liara.run/public/10'; }}
                    />
                    <div>
                      <small className="text-muted">Profile Picture Preview</small>
                    </div>
                  </div>

                  <div className="mb-3">
                    <label className="form-label">Profile Picture (optional)</label>
                    <input
                      type="file"
                      className="form-control"
                      accept="image/*"
                      onChange={handleFileChange}
                    />
                  </div>

                  {/* --- Text Fields --- */}
                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label">First Name</label>
                      <input type="text" className="form-control" name="firstName" value={formData.firstName} onChange={handleChange} required />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label">Last Name</label>
                      <input type="text" className="form-control" name="lastName" value={formData.lastName} onChange={handleChange} required />
                    </div>
                  </div>

                  <div className="mb-3">
                    <label className="form-label">Email</label>
                    <input type="email" className="form-control" name="email" value={formData.email} onChange={handleChange} required />
                  </div>

                  <div className="mb-3">
                    <label className="form-label">Phone Number <span className="text-danger">*</span></label>
                    <input 
                      type="tel" 
                      className="form-control" 
                      name="phoneNumber" 
                      value={formData.phoneNumber} 
                      onChange={handleChange} 
                      placeholder="+20 123 456 7890"
                      required 
                    />
                  </div>

                  <div className="mb-3">
                    <label className="form-label">Password</label>
                    <input type="password" className="form-control" name="password" value={formData.password} onChange={handleChange} required />
                  </div>

                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label">Specialty</label>
                      <input type="text" className="form-control" name="specialty" value={formData.specialty} onChange={handleChange} required />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label">Specialty Tags</label>
                      <input type="text" className="form-control" name="specialtyTags" value={formData.specialtyTags} onChange={handleChange} placeholder="e.g., Cardiology,Heart Disease" />
                    </div>
                  </div>

                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label">Hospital</label>
                      <input type="text" className="form-control" name="hospital" value={formData.hospital} onChange={handleChange} required />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label">City</label>
                      <input type="text" className="form-control" name="city" value={formData.city} onChange={handleChange} required />
                    </div>
                  </div>

                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label">Languages</label>
                      <input type="text" className="form-control" name="languages" value={formData.languages} onChange={handleChange} placeholder="e.g., English,Arabic" required />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label">Years of Experience</label>
                      <input type="number" className="form-control" name="yearsOfExperience" value={formData.yearsOfExperience} onChange={handleChange} required />
                    </div>
                  </div>

                  <div className="mb-3">
                    <label className="form-label">Consultation Fee</label>
                    <input type="number" className="form-control" name="consultationFee" value={formData.consultationFee} onChange={handleChange} required />
                  </div>

                  <div className="mb-3">
                    <label className="form-label">Bio <span className="text-danger">*</span></label>
                    <textarea className="form-control" name="bio" value={formData.bio} onChange={handleChange} rows="3" required></textarea>
                  </div>

                  {/* --- NEW SECTION: National ID Uploads --- */}
                  <div className="card bg-light mb-4">
                    <div className="card-body">
                      <h6 className="card-title mb-3">Identity Verification</h6>
                      <div className="row">
                        <div className="col-md-6 mb-3">
                          <label className="form-label">National ID Front <span className="text-danger">*</span></label>
                          <input 
                            type="file" 
                            className="form-control" 
                            accept="image/*" 
                            onChange={handleIdFrontChange} 
                            required 
                          />
                        </div>
                        <div className="col-md-6 mb-3">
                          <label className="form-label">National ID Back <span className="text-danger">*</span></label>
                          <input 
                            type="file" 
                            className="form-control" 
                            accept="image/*" 
                            onChange={handleIdBackChange} 
                            required 
                          />
                        </div>
                      </div>
                      <small className="text-muted">Please upload clear images of your ID card.</small>
                    </div>
                  </div>
                  {/* -------------------------------------- */}

                  <button type="submit" className="btn btn-primary w-100" disabled={loading}>
                    {loading ? 'Creating Account...' : 'Register as Doctor'}
                  </button>
                </form>

                <div className="text-center mt-3">
                  <p className="mb-0">
                    Already registered? <Link to="/doctor/login">Sign In</Link>
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DoctorRegisterPage;