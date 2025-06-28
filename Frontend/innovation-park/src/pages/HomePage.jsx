import React, { useEffect, useState } from 'react';
import { Box, Heading, Button, SimpleGrid, useDisclosure, Flex, useToast, Spinner, Text } from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import WorkspaceCard from '../components/WorkspaceCard';
import CreateWorkspaceModal from '../components/CreateWorkspaceModal';
import { getWorkspaces } from '../services/api';

const HomePage = () => {
  const [workspaces, setWorkspaces] = useState([]);
  const [loading, setLoading] = useState(true);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const navigate = useNavigate();
  const toast = useToast();

  const fetchWorkspaces = async () => {
    try {
      setLoading(true);
      const data = await getWorkspaces();
      setWorkspaces(data);
    } catch (error) {
      console.error('Erro ao carregar workspaces:', error);
      toast({ 
        title: 'Erro ao carregar workspaces', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWorkspaces();
  }, []);

  if (loading) {
    return (
      <Box p={8} textAlign="center">
        <Spinner size="xl" color="teal.500" />
        <Text mt={4}>Carregando workspaces...</Text>
      </Box>
    );
  }

  return (
    <Box p={8}>
      <Flex justify="space-between" align="center" mb={6}>
        <Heading>Workspaces de Incubação</Heading>
        <Button colorScheme="blue" onClick={() => navigate('/clients')}>
          Gerenciar Clientes
        </Button>
      </Flex>
      <Button colorScheme="teal" onClick={onOpen} mb={6}>Criar Workspace</Button>
      <SimpleGrid columns={[1, 2, 3]} spacing={6}>
        {workspaces.map(ws => (
          <WorkspaceCard key={ws.id} workspace={ws} />
        ))}
      </SimpleGrid>
      <CreateWorkspaceModal isOpen={isOpen} onClose={onClose} onCreated={fetchWorkspaces} />
    </Box>
  );
};

export default HomePage; 