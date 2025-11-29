import Card from './Card';
import Button from './Button';

const SearchFilters = ({ 
  onSearch,
  onClear,
  loading = false,
  children 
}) => {
  return (
    <Card className="mb-4">
      <div className="row g-3 align-items-end">
        {children}
        <div className="col-12 col-md-auto d-flex gap-2">
          <Button onClick={onSearch} loading={loading}>
            Search
          </Button>
          {onClear && (
            <Button variant="outline-secondary" onClick={onClear}>
              Clear
            </Button>
          )}
        </div>
      </div>
    </Card>
  );
};

export default SearchFilters;
