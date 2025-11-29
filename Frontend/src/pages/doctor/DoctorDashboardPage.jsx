import { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { DoctorAuthContext } from '../../context/DoctorAuthContext';
import { useToast } from '../../context/ToastContext';
import { DoctorPageLayout } from '../../components/layouts';
import { Card, PageHeader, StatCard, DataTable, Loading, Modal, Badge, Button, FormTextarea, ConfirmModal } from '../../components/ui';
import { 
  getDoctorAppointments, 
  updateAppointmentNotes, 
  updateAppointmentStatus, 
  deleteAppointment, 
  getPatientMedicalRecords
} from '../../services/doctorService';
import { API_BASE_URL } from '../../config/env';

const DoctorDashboardPage = () => {
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [showDetailsModal, setShowDetailsModal] = useState(false);
  const [doctorNotes, setDoctorNotes] = useState('');
  const [savingNotes, setSavingNotes] = useState(false);
  const [deleting, setDeleting] = useState(null);
  const [medicalRecords, setMedicalRecords] = useState([]);
  const [loadingRecords, setLoadingRecords] = useState(false);
  const [acceptingAppointment, setAcceptingAppointment] = useState(null);
  const [confirmModal, setConfirmModal] = useState({ isOpen: false, appointmentId: null });
  const { doctor } = useContext(DoctorAuthContext);
  const { showSuccess, showError } = useToast();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        const data = await getDoctorAppointments();
        setAppointments(data);
      } catch (error) {
        console.error('Failed to fetch appointments:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchAppointments();
  }, []);

  const handleViewDetails = (appointment) => {
    setSelectedAppointment(appointment);
    setDoctorNotes(appointment.doctorNotes || '');
    setShowDetailsModal(true);
    if (appointment.patientId) loadPatientRecords(appointment.patientId);
  };

  const loadPatientRecords = async (patientId) => {
    try {
      setLoadingRecords(true);
      const records = await getPatientMedicalRecords(patientId);
      setMedicalRecords(records);
    } catch (error) {
      console.error('Failed to load patient records:', error);
      setMedicalRecords([]);
    } finally {
      setLoadingRecords(false);
    }
  };

  const handleSaveNotes = async () => {
    if (!selectedAppointment) return;
    setSavingNotes(true);
    try {
      await updateAppointmentNotes(selectedAppointment.id, { doctorNotes });
      setAppointments(appointments.map(apt => apt.id === selectedAppointment.id ? { ...apt, doctorNotes } : apt));
      setSelectedAppointment({ ...selectedAppointment, doctorNotes });
      showSuccess('Notes saved successfully!');
    } catch (error) {
      console.error('Failed to save notes:', error);
      showError('Failed to save notes. Please try again.');
    } finally {
      setSavingNotes(false);
    }
  };

  const handleChatWithPatient = (patientId, patientName) => {
    navigate('/doctor/chat', { state: { patientId, patientName } });
  };

  const handleDeleteAppointment = (appointmentId) => {
    setConfirmModal({ isOpen: true, appointmentId });
  };

  const handleConfirmDelete = async () => {
    const { appointmentId } = confirmModal;
    setConfirmModal({ isOpen: false, appointmentId: null });

    try {
      setDeleting(appointmentId);
      await deleteAppointment(appointmentId);
      const data = await getDoctorAppointments();
      setAppointments(data);
    } catch (error) {
      console.error('Failed to delete appointment:', error);
    } finally {
      setDeleting(null);
    }
  };

  const closeConfirmModal = () => {
    setConfirmModal({ isOpen: false, appointmentId: null });
  };

  const handleAcceptAppointment = async (appointmentId) => {
    try {
      setAcceptingAppointment(appointmentId);
      await updateAppointmentStatus(appointmentId, 'Confirmed');
      const data = await getDoctorAppointments();
      setAppointments(data);
      showSuccess('Appointment accepted successfully!');
    } catch (error) {
      console.error('Failed to accept appointment:', error);
      showError('Failed to accept appointment. Please try again.');
    } finally {
      setAcceptingAppointment(null);
    }
  };

  const handleCompleteAppointment = async (appointmentId) => {
    try {
      setAcceptingAppointment(appointmentId);
      await updateAppointmentStatus(appointmentId, 'Completed');
      const data = await getDoctorAppointments();
      setAppointments(data);
      showSuccess('Appointment marked as completed!');
    } catch (error) {
      console.error('Failed to complete appointment:', error);
      showError('Failed to complete appointment. Please try again.');
    } finally {
      setAcceptingAppointment(null);
    }
  };

  if (loading) {
    return <DoctorPageLayout><Loading fullPage /></DoctorPageLayout>;
  }

  const appointmentColumns = [
    { key: 'queue', header: 'Queue #', render: (apt) => (
      <div className="text-center">
        <span className="badge bg-info fs-6">{apt.queueNumber > 0 ? apt.queueNumber : 1}</span>
        {apt.totalInSlot > 0 && (
          <small className="d-block text-muted mt-1">of {apt.totalInSlot}</small>
        )}
      </div>
    )},
    { key: 'patient', header: 'Patient', render: (apt) => apt.patientName || 'Unknown Patient' },
    { key: 'date', header: 'Date', render: (apt) => new Date(apt.appointmentDate).toLocaleString() },
    { key: 'type', header: 'Type', render: (apt) => apt.appointmentType },
    { key: 'status', header: 'Status', render: (apt) => (
      <Badge color={apt.status === 'Pending' ? 'warning' : 'success'}>{apt.status}</Badge>
    )},
    { key: 'actions', header: 'Actions', render: (apt) => (
      <div className="d-flex gap-2">
        <Button size="sm" onClick={() => handleViewDetails(apt)}>View Details</Button>
        {apt.status === 'Pending' && (
          <Button size="sm" variant="success" onClick={() => handleAcceptAppointment(apt.id)} disabled={acceptingAppointment === apt.id}>
            {acceptingAppointment === apt.id ? <span className="spinner-border spinner-border-sm"></span> : 'Accept'}
          </Button>
        )}
        {apt.status === 'Confirmed' && (
          <Button size="sm" variant="secondary" onClick={() => handleCompleteAppointment(apt.id)} disabled={acceptingAppointment === apt.id}>
            {acceptingAppointment === apt.id ? <span className="spinner-border spinner-border-sm"></span> : 'Complete'}
          </Button>
        )}
        {apt.patientId && (
          <Button size="sm" variant="outline" onClick={() => handleChatWithPatient(apt.patientId, apt.patientName)}>Chat</Button>
        )}
        <Button size="sm" variant="danger" onClick={() => handleDeleteAppointment(apt.id)} disabled={deleting === apt.id}>
          {deleting === apt.id ? <span className="spinner-border spinner-border-sm"></span> : <i className="bi bi-trash"></i>}
        </Button>
      </div>
    )}
  ];

  return (
    <DoctorPageLayout>
      <PageHeader
        title={`Welcome, Dr. ${doctor?.firstName} ${doctor?.lastName}!`}
        avatar={doctor?.imageUrl}
      />

      <div className="row g-4 mb-4">
        <div className="col-md-4">
          <StatCard value={appointments.length} label="Total Appointments" />
        </div>
        <div className="col-md-4">
          <StatCard value={appointments.filter(a => a.status === 'Pending').length} label="Pending" />
        </div>
        <div className="col-md-4">
          <StatCard value={appointments.filter(a => a.status === 'Completed').length} label="Completed" />
        </div>
      </div>

      <Card title="Appointments">
        <DataTable columns={appointmentColumns} data={appointments} emptyMessage="No appointments yet" />
      </Card>

      <Modal
        show={showDetailsModal}
        onClose={() => setShowDetailsModal(false)}
        title="Appointment Details"
        size="lg"
        footer={
          <>
            <Button variant="secondary" onClick={() => setShowDetailsModal(false)}>Close</Button>
            <Button variant="success" onClick={handleSaveNotes} disabled={savingNotes}>
              {savingNotes ? 'Saving...' : 'Save Notes'}
            </Button>
            {selectedAppointment?.patientId && (
              <Button onClick={() => { setShowDetailsModal(false); handleChatWithPatient(selectedAppointment.patientId, selectedAppointment.patientName); }}>
                Chat with Patient
              </Button>
            )}
          </>
        }
      >
        {selectedAppointment && (
          <>
            <div className="row mb-3">
              <div className="col-md-6"><strong>Patient:</strong> {selectedAppointment.patientName}</div>
              <div className="col-md-6">
                <strong>Patient Phone:</strong>{' '}
                <a href={`tel:${selectedAppointment.patientPhoneNumber}`}>{selectedAppointment.patientPhoneNumber || 'N/A'}</a>
              </div>
            </div>
            <div className="row mb-3">
              <div className="col-md-6">
                <strong>Queue Position:</strong>{' '}
                <span className="badge bg-info fs-6">{selectedAppointment.queueNumber > 0 ? selectedAppointment.queueNumber : 1}</span>
                {selectedAppointment.totalInSlot > 0 && (
                  <span className="text-muted ms-2">of {selectedAppointment.totalInSlot} in this slot</span>
                )}
              </div>
              <div className="col-md-6">
                <strong>Status:</strong>{' '}
                <Badge color={selectedAppointment.status === 'Pending' ? 'warning' : 'success'}>{selectedAppointment.status}</Badge>
              </div>
            </div>
            <div className="row mb-3">
              <div className="col-md-6"><strong>Type:</strong> {selectedAppointment.appointmentType}</div>
              <div className="col-md-6"><strong>Consultation Fee:</strong> ${selectedAppointment.consultationFee}</div>
            </div>
            <div className="row mb-3">
              <div className="col-12"><strong>Date & Time:</strong> {new Date(selectedAppointment.appointmentDate).toLocaleString()}</div>
            </div>
            <div className="mb-3">
              <strong>Patient Notes:</strong>
              <p className="border p-2 mt-2 bg-light">{selectedAppointment.patientNotes || 'No notes provided'}</p>
            </div>
            <FormTextarea
              label="Doctor Notes"
              value={doctorNotes}
              onChange={(e) => setDoctorNotes(e.target.value)}
              rows={3}
              placeholder="Add your notes here..."
            />
            <div className="mt-3">
              <strong>Patient Medical Records:</strong>
              {loadingRecords ? (
                <div className="text-center mt-2"><Loading size="sm" /></div>
              ) : medicalRecords.length > 0 ? (
                <div className="list-group mt-2">
                  {medicalRecords.map((record) => (
                    <div key={record.id} className="list-group-item">
                      <div className="d-flex justify-content-between align-items-start">
                        <div className="flex-grow-1">
                          <h6 className="mb-1">{record.description}<Badge color="secondary" className="ms-2">{record.category}</Badge></h6>
                          <p className="mb-1 small text-muted">Uploaded: {new Date(record.uploadedAt).toLocaleDateString()}</p>
                          <p className="mb-0 small"><strong>File:</strong> {record.fileName}</p>
                        </div>
                        <a href={`${API_BASE_URL}${record.fileUrl}`} target="_blank" rel="noopener noreferrer" className="btn btn-sm btn-outline-primary ms-2">
                          <i className="bi bi-download me-1"></i>View
                        </a>
                      </div>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-muted mt-2">No medical records available for this patient.</p>
              )}
            </div>
          </>
        )}
      </Modal>

      <ConfirmModal
        isOpen={confirmModal.isOpen}
        onCancel={closeConfirmModal}
        onConfirm={handleConfirmDelete}
        title="Delete Appointment"
        message="Are you sure you want to delete this appointment? This action cannot be undone."
        confirmText="Delete"
        variant="danger"
        icon="bi-calendar-x"
      />
    </DoctorPageLayout>
  );
};

export default DoctorDashboardPage;


