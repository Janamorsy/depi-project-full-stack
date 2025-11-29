import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { SearchFilters, ListingCard, Loading, EmptyState, FormInput } from '../components/ui';
import { searchDoctors } from '../services/doctorService';
import { getFileUrl } from '../config/env';

const DoctorsPage = () => {
  const [doctors, setDoctors] = useState([]);
  const [filters, setFilters] = useState({ city: '', specialty: '', language: '' });
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    handleSearch();
  }, []);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const data = await searchDoctors(filters);
      setDoctors(data);
    } catch (error) {
      console.error('Search failed:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleFilterChange = (e) => {
    setFilters({ ...filters, [e.target.name]: e.target.value });
  };

  const handleBookAppointment = (doctor) => {
    navigate('/book-appointment', { state: { doctor } });
  };

  const getDoctorBadges = (doctor) => {
    const badges = [];
    if (doctor.isRecommended) {
      badges.push({ text: 'Recommended for you', variant: 'success', icon: 'bi-star-fill' });
    }
    if (doctor.doctorUserId) {
      badges.push({ text: 'Verified on Platform', variant: 'primary', icon: 'bi-check-circle' });
    } else {
      badges.push({ text: 'External Doctor', variant: 'secondary' });
    }
    return badges;
  };

  const getDoctorDetails = (doctor) => {
    const details = [
      { label: 'Specialty', value: doctor.specialty },
      { value: `${doctor.hospital}, ${doctor.city}` },
      { label: 'Languages', value: doctor.languages },
      { label: 'Experience', value: `${doctor.yearsOfExperience} years` },
      { label: 'Rating', value: `${doctor.rating}/5.0` }
    ];
    return details;
  };

  return (
    <PageLayout title="Find Specialists">
      <SearchFilters onSearch={handleSearch}>
        <div className="col-md-4">
          <FormInput name="city" placeholder="City" value={filters.city} onChange={handleFilterChange} />
        </div>
        <div className="col-md-4">
          <FormInput name="specialty" placeholder="Specialty" value={filters.specialty} onChange={handleFilterChange} />
        </div>
        <div className="col-md-4">
          <FormInput name="language" placeholder="Language" value={filters.language} onChange={handleFilterChange} />
        </div>
      </SearchFilters>

      {loading ? (
        <Loading />
      ) : doctors.length === 0 ? (
        <EmptyState icon="bi-search" title="No doctors found" description="Try adjusting your search filters" />
      ) : (
        <div className="row g-4">
          {doctors.map((doctor, index) => (
            <div key={doctor.doctorUserId || `legacy-${doctor.id}` || index} className="col-md-6">
              <ListingCard
                image={getFileUrl(doctor.imageUrl)}
                title={doctor.name}
                subtitle={doctor.doctorUserId ? '💬 Chat available after booking' : null}
                badges={getDoctorBadges(doctor)}
                details={getDoctorDetails(doctor)}
                price={doctor.consultationFee}
                priceLabel="Consultation Fee"
                actionLabel="Book Appointment"
                onAction={() => handleBookAppointment(doctor)}
                horizontal
                avatarMode
              />
            </div>
          ))}
        </div>
      )}
    </PageLayout>
  );
};

export default DoctorsPage;


