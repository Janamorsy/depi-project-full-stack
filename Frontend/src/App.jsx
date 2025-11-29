import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import { DoctorAuthProvider } from './context/DoctorAuthContext';
import { AdminAuthProvider } from './context/AdminAuthContext';
import { HotelAuthProvider } from './context/HotelAuthContext';
import { ToastProvider } from './context/ToastContext';
import ProtectedRoute from './components/ProtectedRoute';
import DoctorProtectedRoute from './components/DoctorProtectedRoute';
import AdminProtectedRoute from './components/AdminProtectedRoute';
import HotelProtectedRoute from './components/HotelProtectedRoute';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import DashboardPage from './pages/DashboardPage';
import MedicalProfilePage from './pages/MedicalProfilePage';
import DoctorsPage from './pages/DoctorsPage';
import HotelsPage from './pages/HotelsPage';
import TransportPage from './pages/TransportPage';
import BookAppointmentPage from './pages/BookAppointmentPage';
import BookHotelPage from './pages/BookHotelPage';
import BookTransportPage from './pages/BookTransportPage';
import ChatPage from './pages/ChatPage';
import MedicalRecordsPage from './pages/MedicalRecordsPage';
import ProfileSettingsPage from './pages/ProfileSettingsPage';
import AccountPage from './pages/AccountPage';
import DoctorLoginPage from './pages/doctor/DoctorLoginPage';
import DoctorRegisterPage from './pages/doctor/DoctorRegisterPage';
import DoctorDashboardPage from './pages/doctor/DoctorDashboardPage';
import DoctorProfileSettingsPage from './pages/doctor/DoctorProfileSettingsPage';
import DoctorInboxPage from './pages/doctor/DoctorInboxPage';
import InboxPage from './pages/InboxPage';
import PaymentSuccessPage from './pages/PaymentSuccessPage';
import PaymentCancelPage from './pages/PaymentCancelPage';
import AdminLoginPage from './pages/admin/AdminLoginPage';
import AdminDashboardPage from './pages/admin/AdminDashboardPage';
import HotelLoginPage from './pages/hotel/HotelLoginPage';
import HotelRegisterPage from './pages/hotel/HotelRegisterPage';
import HotelDashboardPage from './pages/hotel/HotelDashboardPage';


function App() {
  return (
    <AuthProvider>
      <DoctorAuthProvider>
        <AdminAuthProvider>
          <HotelAuthProvider>
        <ToastProvider>
        <BrowserRouter>
          <Routes>
            
            {/* Admin Routes */}
            <Route path="/admin/login" element={<AdminLoginPage />} />
            <Route
              path="/admin/dashboard"
              element={
                <AdminProtectedRoute>
                  <AdminDashboardPage />
                </AdminProtectedRoute>
              }
            />

            {/* Hotel Owner Routes */}
            <Route path="/hotel/login" element={<HotelLoginPage />} />
            <Route path="/hotel/register" element={<HotelRegisterPage />} />
            <Route
              path="/hotel/dashboard"
              element={
                <HotelProtectedRoute>
                  <HotelDashboardPage />
                </HotelProtectedRoute>
              }
            />
            
            <Route
  path="/inbox"
  element={
    <ProtectedRoute>
      <InboxPage />
    </ProtectedRoute>
  }
/>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            
            <Route path="/doctor/login" element={<DoctorLoginPage />} />
            <Route path="/doctor/register" element={<DoctorRegisterPage />} />
            <Route
              path="/doctor/dashboard"
              element={
                <DoctorProtectedRoute>
                  <DoctorDashboardPage />
                </DoctorProtectedRoute>
              }
            />
            <Route
              path="/doctor/profile-settings"
              element={
                <DoctorProtectedRoute>
                  <DoctorProfileSettingsPage />
                </DoctorProtectedRoute>
              }
            />
            <Route
              path="/doctor/inbox"
              element={
                <DoctorProtectedRoute>
                  <DoctorInboxPage />
                </DoctorProtectedRoute>
              }
            />
            <Route
              path="/doctor/chat/:userId"
              element={
                <DoctorProtectedRoute>
                  <ChatPage />
                </DoctorProtectedRoute>
              }
            />
            <Route
              path="/doctor/chat"
              element={
                <DoctorProtectedRoute>
                  <ChatPage />
                </DoctorProtectedRoute>
              }
            />
            
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <DashboardPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/profile"
              element={
                <ProtectedRoute>
                  <MedicalProfilePage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/account"
              element={
                <ProtectedRoute>
                  <AccountPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/profile-settings"
              element={
                <ProtectedRoute>
                  <ProfileSettingsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/doctors"
              element={
                <ProtectedRoute>
                  <DoctorsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/hotels"
              element={
                <ProtectedRoute>
                  <HotelsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/transport"
              element={
                <ProtectedRoute>
                  <TransportPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/book-appointment"
              element={
                <ProtectedRoute>
                  <BookAppointmentPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/book-hotel"
              element={
                <ProtectedRoute>
                  <BookHotelPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/book-transport"
              element={
                <ProtectedRoute>
                  <BookTransportPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/chat/:userId"
              element={
                <ProtectedRoute>
                  <ChatPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/chat"
              element={
                <ProtectedRoute>
                  <ChatPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/medical-records"
              element={
                <ProtectedRoute>
                  <MedicalRecordsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/payment/success"
              element={
                <ProtectedRoute>
                  <PaymentSuccessPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/payment/cancel"
              element={
                <ProtectedRoute>
                  <PaymentCancelPage />
                </ProtectedRoute>
              }
            />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </BrowserRouter>
        </ToastProvider>
          </HotelAuthProvider>
        </AdminAuthProvider>
      </DoctorAuthProvider>
    </AuthProvider>
  );
}

export default App;

