import { useState } from 'react';
import { Button, FormSelect } from './index';

const DAYS_OF_WEEK = [
  { value: 0, label: 'Sunday' },
  { value: 1, label: 'Monday' },
  { value: 2, label: 'Tuesday' },
  { value: 3, label: 'Wednesday' },
  { value: 4, label: 'Thursday' },
  { value: 5, label: 'Friday' },
  { value: 6, label: 'Saturday' }
];

const TIME_OPTIONS = [];
for (let h = 6; h <= 22; h++) {
  for (let m = 0; m < 60; m += 30) {
    const time = `${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`;
    const label = new Date(`2000-01-01T${time}`).toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' });
    TIME_OPTIONS.push({ value: time, label });
  }
}

const AvailabilityScheduler = ({ slots = [], onChange, saving = false }) => {
  const [editingSlot, setEditingSlot] = useState(null);

  const getDayName = (dayNum) => DAYS_OF_WEEK.find(d => d.value === dayNum)?.label || 'Unknown';

  const handleAddSlot = () => {
    setEditingSlot({ day: 1, start: '09:00', end: '17:00', isNew: true });
  };

  const handleEditSlot = (index) => {
    setEditingSlot({ ...slots[index], index });
  };

  const handleDeleteSlot = (index) => {
    const newSlots = slots.filter((_, i) => i !== index);
    onChange(newSlots);
  };

  const handleSaveSlot = () => {
    if (!editingSlot) return;

    const { day, start, end, isNew, index } = editingSlot;
    
    if (start >= end) {
      return; // Invalid time range
    }

    const newSlot = { day: parseInt(day), start, end };
    
    let newSlots;
    if (isNew) {
      newSlots = [...slots, newSlot];
    } else {
      newSlots = slots.map((s, i) => i === index ? newSlot : s);
    }

    // Sort by day then start time
    newSlots.sort((a, b) => a.day - b.day || a.start.localeCompare(b.start));
    
    onChange(newSlots);
    setEditingSlot(null);
  };

  const handleCancelEdit = () => {
    setEditingSlot(null);
  };

  const groupedSlots = DAYS_OF_WEEK.map(day => ({
    ...day,
    slots: slots.filter(s => s.day === day.value)
  })).filter(day => day.slots.length > 0);

  return (
    <div className="availability-scheduler">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h6 className="mb-0">
          <i className="bi bi-calendar-week me-2"></i>
          Weekly Schedule
        </h6>
        <Button size="sm" onClick={handleAddSlot} disabled={saving}>
          <i className="bi bi-plus-lg me-1"></i>Add Time Slot
        </Button>
      </div>

      {/* Current Schedule */}
      {slots.length === 0 ? (
        <div className="alert alert-info">
          <i className="bi bi-info-circle me-2"></i>
          No availability set. Click "Add Time Slot" to set your working hours.
        </div>
      ) : (
        <div className="schedule-grid">
          {groupedSlots.map(day => (
            <div key={day.value} className="card mb-2">
              <div className="card-body py-2 px-3">
                <div className="d-flex justify-content-between align-items-center">
                  <strong className="text-primary">
                    <i className="bi bi-calendar-day me-2"></i>
                    {day.label}
                  </strong>
                  <div className="d-flex flex-wrap gap-2">
                    {day.slots.map((slot, idx) => {
                      const globalIndex = slots.findIndex(s => s === slot);
                      return (
                        <div key={idx} className="badge bg-success-subtle text-success d-flex align-items-center gap-2 py-2 px-3">
                          <i className="bi bi-clock"></i>
                          <span>{slot.start} - {slot.end}</span>
                          <button 
                            className="btn btn-link btn-sm p-0 text-primary" 
                            onClick={() => handleEditSlot(globalIndex)}
                            disabled={saving}
                          >
                            <i className="bi bi-pencil"></i>
                          </button>
                          <button 
                            className="btn btn-link btn-sm p-0 text-danger" 
                            onClick={() => handleDeleteSlot(globalIndex)}
                            disabled={saving}
                          >
                            <i className="bi bi-x-lg"></i>
                          </button>
                        </div>
                      );
                    })}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Edit/Add Modal */}
      {editingSlot && (
        <div className="modal show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  <i className="bi bi-clock me-2"></i>
                  {editingSlot.isNew ? 'Add Time Slot' : 'Edit Time Slot'}
                </h5>
                <button type="button" className="btn-close" onClick={handleCancelEdit}></button>
              </div>
              <div className="modal-body">
                <div className="mb-3">
                  <label className="form-label">Day of Week</label>
                  <select 
                    className="form-select"
                    value={editingSlot.day}
                    onChange={(e) => setEditingSlot({ ...editingSlot, day: parseInt(e.target.value) })}
                  >
                    {DAYS_OF_WEEK.map(day => (
                      <option key={day.value} value={day.value}>{day.label}</option>
                    ))}
                  </select>
                </div>
                <div className="row">
                  <div className="col-6">
                    <label className="form-label">Start Time</label>
                    <select 
                      className="form-select"
                      value={editingSlot.start}
                      onChange={(e) => setEditingSlot({ ...editingSlot, start: e.target.value })}
                    >
                      {TIME_OPTIONS.map(t => (
                        <option key={t.value} value={t.value}>{t.label}</option>
                      ))}
                    </select>
                  </div>
                  <div className="col-6">
                    <label className="form-label">End Time</label>
                    <select 
                      className="form-select"
                      value={editingSlot.end}
                      onChange={(e) => setEditingSlot({ ...editingSlot, end: e.target.value })}
                    >
                      {TIME_OPTIONS.map(t => (
                        <option key={t.value} value={t.value}>{t.label}</option>
                      ))}
                    </select>
                  </div>
                </div>
                {editingSlot.start >= editingSlot.end && (
                  <div className="alert alert-warning mt-3 mb-0 py-2">
                    <i className="bi bi-exclamation-triangle me-2"></i>
                    End time must be after start time
                  </div>
                )}
              </div>
              <div className="modal-footer">
                <Button variant="secondary" onClick={handleCancelEdit}>Cancel</Button>
                <Button 
                  onClick={handleSaveSlot} 
                  disabled={editingSlot.start >= editingSlot.end}
                >
                  <i className="bi bi-check-lg me-1"></i>
                  {editingSlot.isNew ? 'Add Slot' : 'Save Changes'}
                </Button>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Quick Add Buttons */}
      <div className="mt-3 pt-3 border-top">
        <small className="text-muted d-block mb-2">Quick Templates:</small>
        <div className="d-flex flex-wrap gap-2">
          <button 
            className="btn btn-outline-secondary btn-sm"
            onClick={() => onChange([
              { day: 1, start: '09:00', end: '17:00' },
              { day: 2, start: '09:00', end: '17:00' },
              { day: 3, start: '09:00', end: '17:00' },
              { day: 4, start: '09:00', end: '17:00' },
              { day: 5, start: '09:00', end: '17:00' }
            ])}
            disabled={saving}
          >
            <i className="bi bi-calendar-week me-1"></i>Mon-Fri 9-5
          </button>
          <button 
            className="btn btn-outline-secondary btn-sm"
            onClick={() => onChange([
              { day: 0, start: '10:00', end: '14:00' },
              { day: 1, start: '09:00', end: '17:00' },
              { day: 2, start: '09:00', end: '17:00' },
              { day: 3, start: '09:00', end: '17:00' },
              { day: 4, start: '09:00', end: '17:00' },
              { day: 6, start: '10:00', end: '14:00' }
            ])}
            disabled={saving}
          >
            <i className="bi bi-calendar-check me-1"></i>6 Days Week
          </button>
          <button 
            className="btn btn-outline-danger btn-sm"
            onClick={() => onChange([])}
            disabled={saving || slots.length === 0}
          >
            <i className="bi bi-trash me-1"></i>Clear All
          </button>
        </div>
      </div>
    </div>
  );
};

export default AvailabilityScheduler;
