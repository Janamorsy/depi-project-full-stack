import Card from './Card';

const StatCard = ({ value, label, icon, variant = 'primary' }) => {
  return (
    <Card className="h-100">
      <div className="text-center">
        {icon && <i className={`bi ${icon} display-6 text-${variant} mb-2 d-block`}></i>}
        <h3 className="display-4 mb-0">{value}</h3>
        <p className="text-muted mb-0">{label}</p>
      </div>
    </Card>
  );
};

export default StatCard;
