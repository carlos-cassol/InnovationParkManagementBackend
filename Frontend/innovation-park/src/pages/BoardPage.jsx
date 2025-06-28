import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Box, Heading, Flex, Button, useDisclosure, useToast, Spinner, Text } from '@chakra-ui/react';
import { getWorkAreaWithColumns } from '../services/api';
import KanbanBoard from '../components/KanbanBoard';
import CreateColumnModal from '../components/CreateColumnModal';

const BoardPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [workArea, setWorkArea] = useState(null);
  const [loading, setLoading] = useState(true);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const toast = useToast();

  const fetchWorkArea = async () => {
    try {
      setLoading(true);
      const data = await getWorkAreaWithColumns(id);
      setWorkArea(data);
    } catch (error) {
      console.error('Erro ao carregar workspace:', error);
      toast({ 
        title: 'Erro ao carregar workspace', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWorkArea();
  }, [id]);

  if (loading) {
    return (
      <Box p={8} textAlign="center">
        <Spinner size="xl" color="teal.500" />
        <Text mt={4}>Carregando workspace...</Text>
      </Box>
    );
  }

  if (!workArea) {
    return (
      <Box p={8} textAlign="center">
        <Text>Workspace não encontrado</Text>
        <Button mt={4} colorScheme="blue" onClick={() => navigate('/')}>
          Voltar aos Workspaces
        </Button>
      </Box>
    );
  }

  return (
    <Box>
      <Flex align="center" justify="space-between" mb={4} p={4} bg="white" boxShadow="sm">
        <Flex align="center" gap={4}>
          <Button colorScheme="gray" onClick={() => navigate('/')}>
            ← Voltar aos Workspaces
          </Button>
          <Heading size="lg">{workArea.name}</Heading>
        </Flex>
        <Button colorScheme="teal" onClick={onOpen}>Adicionar Coluna</Button>
      </Flex>
      
      <KanbanBoard workArea={workArea} onRefresh={fetchWorkArea} />
      
      <CreateColumnModal isOpen={isOpen} onClose={onClose} workAreaId={id} onCreated={fetchWorkArea} />
    </Box>
  );
};

export default BoardPage; 