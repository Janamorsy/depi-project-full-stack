import EmptyState from './EmptyState';
import Loading from './Loading';

const DataTable = ({ 
  columns, 
  data, 
  loading = false,
  emptyMessage = 'No data found',
  emptyIcon = 'bi-inbox',
  keyField = 'id',
  className = ''
}) => {
  if (loading) {
    return <Loading />;
  }

  if (!data || data.length === 0) {
    return <EmptyState title={emptyMessage} icon={emptyIcon} />;
  }

  return (
    <div className={`table-responsive ${className}`}>
      <table className="table">
        <thead>
          <tr>
            {columns.map((col, idx) => (
              <th key={col.key || idx} className={col.headerClassName || ''}>
                {col.header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row, rowIdx) => (
            <tr key={row[keyField] || rowIdx}>
              {columns.map((col, colIdx) => (
                <td key={col.key || colIdx} className={col.cellClassName || ''}>
                  {col.render ? col.render(row, rowIdx) : row[col.key]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DataTable;
