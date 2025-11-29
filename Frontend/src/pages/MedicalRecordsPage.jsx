import { useState, useEffect } from 'react';
import { PageLayout } from '../components/layouts';
import { Card, Button, Loading, EmptyState, Badge, FormSelect, FormTextarea, ConfirmModal } from '../components/ui';
import { getMedicalRecords, uploadMedicalRecord, deleteMedicalRecord } from '../services/medicalRecordService';
import { API_BASE_URL } from '../config/env';
import { useToast } from '../context/ToastContext';

const CATEGORY_OPTIONS = [
  { value: 'Lab Results', label: 'Lab Results' },
  { value: 'X-Ray', label: 'X-Ray' },
  { value: 'MRI/CT Scan', label: 'MRI/CT Scan' },
  { value: 'Prescription', label: 'Prescription' },
  { value: 'Report', label: 'Medical Report' },
  { value: 'Other', label: 'Other' }
];

const FILTER_OPTIONS = [
  { value: 'All', label: 'All Categories' },
  ...CATEGORY_OPTIONS
];

const MedicalRecordsPage = () => {
  const [records, setRecords] = useState([]);
  const [loading, setLoading] = useState(true);
  const [uploading, setUploading] = useState(false);
  const [file, setFile] = useState(null);
  const [description, setDescription] = useState('');
  const [category, setCategory] = useState('Lab Results');
  const [filterCategory, setFilterCategory] = useState('All');
  const [confirmModal, setConfirmModal] = useState({ isOpen: false, recordId: null });
  const { showSuccess, showError } = useToast();

  useEffect(() => {
    fetchRecords();
  }, []);

  const fetchRecords = async () => {
    try {
      const data = await getMedicalRecords();
      setRecords(data);
    } catch (error) {
      console.error('Failed to fetch records:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
  };

  const handleUpload = async (e) => {
    e.preventDefault();
    if (!file) return;

    setUploading(true);
    try {
      await uploadMedicalRecord(file, description, category);
      setFile(null);
      setDescription('');
      setCategory('Lab Results');
      fetchRecords();
      showSuccess('File uploaded successfully!');
    } catch (error) {
      showError('Failed to upload file');
    } finally {
      setUploading(false);
    }
  };

  const handleDelete = async (id) => {
    setConfirmModal({ isOpen: true, recordId: id });
  };

  const handleConfirmDelete = async () => {
    const { recordId } = confirmModal;
    setConfirmModal({ isOpen: false, recordId: null });

    try {
      await deleteMedicalRecord(recordId);
      fetchRecords();
    } catch (error) {
      console.error('Failed to delete record:', error);
    }
  };

  const closeConfirmModal = () => {
    setConfirmModal({ isOpen: false, recordId: null });
  };

  const filteredRecords = filterCategory === 'All'
    ? records
    : records.filter((r) => r.category === filterCategory);

  const renderUploadForm = () => (
    <Card title="Upload New Record" icon="bi-cloud-upload" className="mb-4">
      <form onSubmit={handleUpload}>
        <div className="mb-3">
          <label className="form-label">File</label>
          <input type="file" className="form-control" onChange={handleFileChange} accept=".pdf,.jpg,.jpeg,.png" required />
          <small className="form-text text-muted">Accepted: PDF, JPG, PNG (Max 10MB)</small>
        </div>

        <FormSelect label="Category" value={category} onChange={(e) => setCategory(e.target.value)} options={CATEGORY_OPTIONS} />

        <FormTextarea label="Description" value={description} onChange={(e) => setDescription(e.target.value)} rows={2} placeholder="Brief description..." required />

        <Button type="submit" className="w-100" disabled={uploading || !file || !description.trim()} loading={uploading}>
          {uploading ? 'Uploading...' : <><i className="bi bi-upload me-2"></i>Upload</>}
        </Button>
      </form>
    </Card>
  );

  const renderRecordsTable = () => (
    <Card title="My Records" icon="bi-folder2-open">
      {loading ? (
        <Loading text="Loading records..." />
      ) : records.length === 0 ? (
        <EmptyState icon="bi-file-earmark-medical" title="No records yet" description="Upload your first medical record to get started." />
      ) : (
        <>
          <div className="mb-3">
            <FormSelect label="Filter By Category" value={filterCategory} onChange={(e) => setFilterCategory(e.target.value)} options={FILTER_OPTIONS} className="w-50" />
          </div>

          <div className="table-responsive">
            <table className="table table-hover">
              <thead className="table-light">
                <tr>
                  <th>Category</th>
                  <th>Uploaded</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredRecords.map((record) => (
                  <tr key={record.id}>
                    <td><Badge variant="info">{record.category}</Badge></td>
                    <td>{new Date(record.uploadedAt).toLocaleDateString()}</td>
                    <td>
                      <a href={`${API_BASE_URL}${record.fileUrl}`} target="_blank" rel="noopener noreferrer" className="btn btn-sm btn-primary me-1">
                        <i className="bi bi-eye me-1"></i>View
                      </a>
                      <Button variant="danger" size="sm" onClick={() => handleDelete(record.id)}>
                        <i className="bi bi-trash me-1"></i>Delete
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {filteredRecords.length === 0 && (
            <p className="text-muted text-center">No records match the selected filter.</p>
          )}
        </>
      )}
    </Card>
  );

  return (
    <PageLayout title="Medical Records" subtitle="Upload and manage your medical documents">
      <div className="row">
        <div className="col-md-4">{renderUploadForm()}</div>
        <div className="col-md-8">{renderRecordsTable()}</div>
      </div>

      <ConfirmModal
        isOpen={confirmModal.isOpen}
        onCancel={closeConfirmModal}
        onConfirm={handleConfirmDelete}
        title="Delete Medical Record"
        message="Are you sure you want to delete this medical record? This action cannot be undone."
        confirmText="Delete"
        variant="danger"
        icon="bi-file-earmark-x"
      />
    </PageLayout>
  );
};

export default MedicalRecordsPage;


