import axios from 'axios';

const API_URL = 'http://localhost:5297/api'; // Ajuste conforme sua porta/backend

// Configurar axios com interceptors
const api = axios.create({
  baseURL: API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para requisições
api.interceptors.request.use(
  (config) => {
    console.log('Requisição:', config.method?.toUpperCase(), config.url);
    return config;
  },
  (error) => {
    console.error('Erro na requisição:', error);
    return Promise.reject(error);
  },
);

// Interceptor para respostas
api.interceptors.response.use(
  (response) => {
    console.log('Resposta:', response.status, response.config.url);
    return response;
  },
  (error) => {
    console.error(
      'Erro na resposta:',
      error.response?.status,
      error.response?.data,
    );
    return Promise.reject(error);
  },
);

// Workspaces (áreas de trabalho)
export const getWorkspaces = async () => {
  const { data } = await api.get('/WorkArea');
  return data;
};

export const createWorkspace = async (payload) => {
  const { data } = await api.post('/WorkArea', payload);
  return data;
};

export const getWorkAreaWithColumns = async (id) => {
  const { data } = await api.get(`/WorkArea/${id}`);
  return data;
};

// Colunas
export const createColumn = async (payload) => {
  const { data } = await api.post('/Column', payload);
  return data;
};

export const reorderColumn = async (columnId, newOrder) => {
  console.log(
    `Chamando reorderColumn: columnId=${columnId}, newOrder=${newOrder}`,
  );
  const { data } = await api.patch(`/Column/${columnId}/reorder`, newOrder);
  console.log('Resposta do reorderColumn:', data);
  return data;
};

// Cards
export const createCard = async (payload) => {
  // Converter os valores do enum para os valores corretos do backend
  const createCardDto = {
    ...payload,
    incubationPlan: convertIncubationPlanToBackend(payload.incubationPlan),
  };

  const { data } = await api.post('/Card', createCardDto);
  return data;
};

export const updateCard = async (id, payload) => {
  // Converter os valores do enum para os valores corretos do backend
  const updateCardDto = {
    ...payload,
    incubationPlan: convertIncubationPlanToBackend(payload.incubationPlan),
  };

  const { data } = await api.put(`/Card/${id}`, updateCardDto);
  return data;
};

export const moveCard = async (cardId, newColumnId) => {
  const { data } = await api.patch(`/Card/${cardId}/move`, newColumnId);
  return data;
};

export const moveCardWithIndex = async (cardId, newColumnId, newIndex) => {
  const { data } = await api.patch(`/Card/${cardId}/move-with-index`, {
    newColumnId,
    newIndex,
  });
  return data;
};

// Card Files
export const uploadCardFile = async (formData) => {
  console.log('Iniciando upload de arquivo:', {
    cardId: formData.get('cardId'),
    isPaymentReceipt: formData.get('isPaymentReceipt'),
    fileName: formData.get('file')?.name,
    fileSize: formData.get('file')?.size,
  });

  const { data } = await api.post('/CardFile/upload', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });

  console.log('Upload concluído:', data);
  return data;
};

export const getCardFiles = async (cardId) => {
  console.log('Buscando arquivos do card:', cardId);
  const { data } = await api.get(`/CardFile/card/${cardId}`);
  console.log('Arquivos encontrados:', data);
  return data;
};

// Clientes
export const getClients = async () => {
  const { data } = await api.get('/Client');
  return data;
};

export const createClient = async (payload) => {
  const { data } = await api.post('/Client', payload);
  return data;
};

// Função auxiliar para converter valores do enum
const convertIncubationPlanToBackend = (frontendValue) => {
  switch (frontendValue) {
    case 'BASIC':
    case 'Start':
      return 1; // Start
    case 'ADVANCED':
    case 'Grow':
      return 2; // Grow
    default:
      return 1; // Default para Start
  }
};

// Função auxiliar para converter valores do backend para o frontend
export const convertIncubationPlanFromBackend = (backendValue) => {
  switch (backendValue) {
    case 1:
      return 'Start';
    case 2:
      return 'Grow';
    default:
      return 'Start';
  }
};
