import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage';
import BoardPage from './pages/BoardPage';
import ClientsPage from './pages/ClientsPage';
import NotFoundPage from './pages/NotFoundPage';

// Error Boundary Component melhorado
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null, errorInfo: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error('Erro capturado pelo Error Boundary:', error);
    console.error('Stack trace:', errorInfo);
    this.setState({ errorInfo });
  }

  render() {
    if (this.state.hasError) {
      return (
        <div
          style={{
            padding: '20px',
            textAlign: 'center',
            fontFamily: 'Arial, sans-serif',
            maxWidth: '600px',
            margin: '50px auto',
          }}
        >
          <h1 style={{ color: '#e53e3e', marginBottom: '20px' }}>
            Algo deu errado!
          </h1>
          <p style={{ marginBottom: '20px' }}>
            Ocorreu um erro inesperado. Por favor, recarregue a página.
          </p>
          {this.state.error && (
            <details
              style={{
                textAlign: 'left',
                marginBottom: '20px',
                padding: '10px',
                backgroundColor: '#f7fafc',
                borderRadius: '5px',
              }}
            >
              <summary>Detalhes do erro</summary>
              <pre
                style={{
                  whiteSpace: 'pre-wrap',
                  fontSize: '12px',
                  color: '#e53e3e',
                }}
              >
                {this.state.error.toString()}
              </pre>
            </details>
          )}
          <button
            onClick={() => window.location.reload()}
            style={{
              padding: '10px 20px',
              backgroundColor: '#3182ce',
              color: 'white',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
              fontSize: '16px',
            }}
          >
            Recarregar Página
          </button>
        </div>
      );
    }

    return this.props.children;
  }
}

function App() {
  return (
    <ErrorBoundary>
      <ChakraProvider>
        <Router>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/workspace/:id" element={<BoardPage />} />
            <Route path="/clients" element={<ClientsPage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Router>
      </ChakraProvider>
    </ErrorBoundary>
  );
}

export default App;
