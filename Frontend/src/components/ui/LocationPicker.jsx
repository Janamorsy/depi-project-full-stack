import { useEffect, useRef, useState } from 'react';
import 'ol/ol.css';

const CAIRO_CENTER = [31.2357, 30.0444]; // Cairo coordinates [lon, lat]
const MESSAGE_DURATION = 3000;

const LocationPicker = ({ 
  pickupLocation, 
  dropoffLocation, 
  onPickupChange, 
  onDropoffChange
}) => {
  const mapContainerRef = useRef(null);
  const mapInstanceRef = useRef(null);
  const pickupSourceRef = useRef(null);
  const dropoffSourceRef = useRef(null);
  const olModulesRef = useRef(null);
  const [selectionMode, setSelectionMode] = useState(null); // null = nothing selected initially
  const [pickupSet, setPickupSet] = useState(false);
  const [dropoffSet, setDropoffSet] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [message, setMessage] = useState(null); // { type: 'error'|'warning', text: string }

  const showMessage = (text, type = 'warning') => {
    setMessage({ text, type });
    setTimeout(() => setMessage(null), MESSAGE_DURATION);
  };

  const reverseGeocode = async (lat, lon) => {
    try {
      const response = await fetch(
        `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lon}&zoom=18&addressdetails=1`,
        { headers: { 'Accept-Language': 'en' } }
      );
      const data = await response.json();
      return data.display_name || `${lat.toFixed(6)}, ${lon.toFixed(6)}`;
    } catch (error) {
      return `${lat.toFixed(6)}, ${lon.toFixed(6)}`;
    }
  };

  // Initialize map once
  useEffect(() => {
    let isMounted = true;

    const initMap = async () => {
      if (!mapContainerRef.current || mapInstanceRef.current) return;

      try {
        // Load OpenLayers modules
        const [
          { default: Map },
          { default: View },
          { default: TileLayer },
          { default: VectorLayer },
          { default: VectorSource },
          { default: OSM },
          proj,
          { default: Feature },
          { default: Point },
          style
        ] = await Promise.all([
          import('ol/Map'),
          import('ol/View'),
          import('ol/layer/Tile'),
          import('ol/layer/Vector'),
          import('ol/source/Vector'),
          import('ol/source/OSM'),
          import('ol/proj'),
          import('ol/Feature'),
          import('ol/geom/Point'),
          import('ol/style')
        ]);

        if (!isMounted) return;

        // Store modules for later use
        olModulesRef.current = { Feature, Point, proj };

        const createMarkerStyle = (color) => new style.Style({
          image: new style.Circle({
            radius: 12,
            fill: new style.Fill({ color }),
            stroke: new style.Stroke({ color: '#fff', width: 3 })
          })
        });

        // Create sources
        const pickupSource = new VectorSource();
        const dropoffSource = new VectorSource();
        pickupSourceRef.current = pickupSource;
        dropoffSourceRef.current = dropoffSource;

        // Create map
        const map = new Map({
          target: mapContainerRef.current,
          layers: [
            new TileLayer({ source: new OSM() }),
            new VectorLayer({ source: pickupSource, style: createMarkerStyle('#198754') }),
            new VectorLayer({ source: dropoffSource, style: createMarkerStyle('#dc3545') })
          ],
          view: new View({
            center: proj.fromLonLat(CAIRO_CENTER),
            zoom: 12
          })
        });

        mapInstanceRef.current = map;
        setIsLoading(false);

      } catch (error) {
        console.error('Failed to initialize map:', error);
        setIsLoading(false);
      }
    };

    initMap();

    return () => {
      isMounted = false;
      if (mapInstanceRef.current) {
        mapInstanceRef.current.setTarget(null);
        mapInstanceRef.current = null;
      }
    };
  }, []);

  // Handle map clicks - separate effect to avoid recreating map
  useEffect(() => {
    const map = mapInstanceRef.current;
    if (!map || !olModulesRef.current) return;

    const { Feature, Point, proj } = olModulesRef.current;

    const handleClick = async (event) => {
      if (!selectionMode) return; // Don't do anything if no mode selected
      
      const [lon, lat] = proj.toLonLat(event.coordinate);
      const address = await reverseGeocode(lat, lon);

      const addMarker = (source) => {
        source.clear();
        source.addFeature(new Feature({ geometry: new Point(event.coordinate) }));
      };

      if (selectionMode === 'pickup') {
        addMarker(pickupSourceRef.current);
        onPickupChange(address);
        setPickupSet(true);
      } else {
        addMarker(dropoffSourceRef.current);
        onDropoffChange(address);
        setDropoffSet(true);
      }
    };

    map.on('click', handleClick);
    return () => map.un('click', handleClick);
  }, [selectionMode, onPickupChange, onDropoffChange]);

  const handleLocateMe = () => {
    if (!selectionMode) {
      showMessage('Please select "Set Pickup" or "Set Dropoff" first.');
      return;
    }
    
    if (!navigator.geolocation) {
      showMessage('Geolocation is not supported by your browser.', 'error');
      return;
    }

    const map = mapInstanceRef.current;
    const modules = olModulesRef.current;
    if (!map || !modules) return;

    navigator.geolocation.getCurrentPosition(
      async (position) => {
        const { latitude, longitude } = position.coords;
        const { Feature, Point, proj } = modules;
        const coordinate = proj.fromLonLat([longitude, latitude]);

        map.getView().animate({ center: coordinate, zoom: 15, duration: 500 });

        const address = await reverseGeocode(latitude, longitude);

        const addMarker = (source) => {
          source.clear();
          source.addFeature(new Feature({ geometry: new Point(coordinate) }));
        };

        if (selectionMode === 'pickup') {
          addMarker(pickupSourceRef.current);
          onPickupChange(address);
          setPickupSet(true);
        } else {
          addMarker(dropoffSourceRef.current);
          onDropoffChange(address);
          setDropoffSet(true);
        }
      },
      () => showMessage('Unable to get your location.', 'error')
    );
  };

  return (
    <div className="location-picker">
      <div className="d-flex gap-2 mb-3">
        <button
          type="button"
          className={`btn flex-fill ${selectionMode === 'pickup' ? 'btn-success' : 'btn-outline-success'}`}
          onClick={() => setSelectionMode('pickup')}
        >
          <i className="bi bi-geo-alt-fill me-2"></i>
          Set Pickup
        </button>
        <button
          type="button"
          className={`btn flex-fill ${selectionMode === 'dropoff' ? 'btn-danger' : 'btn-outline-danger'}`}
          onClick={() => setSelectionMode('dropoff')}
        >
          <i className="bi bi-geo-alt me-2"></i>
          Set Dropoff
        </button>
        <button
          type="button"
          className="btn btn-outline-primary"
          onClick={handleLocateMe}
          disabled={isLoading || !selectionMode}
          title={selectionMode ? "Use my current location" : "Select pickup or dropoff first"}
        >
          <i className="bi bi-cursor-fill"></i>
        </button>
      </div>

      <div className="alert alert-info py-2 mb-3">
        <small>
          <i className="bi bi-info-circle me-2"></i>
          {selectionMode 
            ? <>Click on the map to set your <strong>{selectionMode}</strong> location</>
            : <>Select <strong>Set Pickup</strong> or <strong>Set Dropoff</strong> button, then click on the map</>
          }
        </small>
      </div>

      {message && (
        <div className={`alert py-2 mb-3 ${message.type === 'error' ? 'alert-danger' : 'alert-warning'}`}>
          <small>
            <i className={`bi ${message.type === 'error' ? 'bi-x-circle' : 'bi-exclamation-triangle'} me-2`}></i>
            {message.text}
          </small>
        </div>
      )}

      <div 
        ref={mapContainerRef} 
        className="rounded border"
        style={{ height: '350px', width: '100%', background: '#e5e3df' }}
      />

      <div className="mt-3">
        <div className="row g-2">
          <div className="col-6">
            <div className={`p-2 rounded border ${pickupSet ? 'border-success bg-success bg-opacity-10' : ''}`}>
              <small className="text-success fw-bold d-block">
                <i className="bi bi-geo-alt-fill me-1"></i>Pickup
              </small>
              <small className="text-truncate d-block" style={{ maxWidth: '100%' }}>
                {pickupLocation || 'Click map to select'}
              </small>
            </div>
          </div>
          <div className="col-6">
            <div className={`p-2 rounded border ${dropoffSet ? 'border-danger bg-danger bg-opacity-10' : ''}`}>
              <small className="text-danger fw-bold d-block">
                <i className="bi bi-geo-alt me-1"></i>Dropoff
              </small>
              <small className="text-truncate d-block" style={{ maxWidth: '100%' }}>
                {dropoffLocation || 'Click map to select'}
              </small>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LocationPicker;
