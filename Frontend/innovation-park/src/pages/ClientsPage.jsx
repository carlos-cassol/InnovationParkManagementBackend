import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Heading, Button, Table, Thead, Tbody, Tr, Th, Td, useDisclosure, useToast, Flex } from '@chakra-ui/react';
import { getClients } from '../services/api';
import CreateClientModal from '../components/CreateClientModal';

const ClientsPage = () => {
  const [clients, setClients] = useState([]);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const toast = useToast();
  const navigate = useNavigate();

  const fetchClients = async () => {
    try {
      const data = await getClients();
      setClients(data);
    } catch (error) {
      console.error('Error fetching clients:', error);
      toast({ 
        title: 'Erro ao carregar clientes', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    }
  };

  useEffect(() => {
    fetchClients();
  }, []);

  return (
    <Box p={8}>
      <Flex justify="space-between" align="center" mb={6}>
        <Flex align="center" gap={4}>
          <Button colorScheme="gray" onClick={() => navigate('/')}>
            ← Voltar aos Workspaces
          </Button>
          <Heading>Gerenciamento de Clientes</Heading>
        </Flex>
        <Button colorScheme="teal" onClick={onOpen}>Criar Cliente</Button>
      </Flex>
      
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>Nome</Th>
            <Th>CPF/CNPJ</Th>
            <Th>Contato</Th>
            <Th>Endereço</Th>
            <Th>Data de Registro</Th>
          </Tr>
        </Thead>
        <Tbody>
          {clients.map(client => (
            <Tr key={client.id}>
              <Td>{client.name}</Td>
              <Td>{client.cpfCnpj}</Td>
              <Td>{client.contact}</Td>
              <Td>{client.address}</Td>
              <Td>{new Date(client.registrationDate).toLocaleDateString()}</Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
      
      <CreateClientModal isOpen={isOpen} onClose={onClose} onCreated={fetchClients} />
    </Box>
  );
};

export default ClientsPage; 