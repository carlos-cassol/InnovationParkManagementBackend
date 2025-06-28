import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Heading, Text, Button } from '@chakra-ui/react';

const NotFoundPage = () => {
  const navigate = useNavigate();
  
  return (
    <Box p={8} textAlign="center">
      <Heading>404</Heading>
      <Text mb={4}>Página não encontrada.</Text>
      <Button colorScheme="teal" onClick={() => navigate('/')}>
        Voltar ao Início
      </Button>
    </Box>
  );
};

export default NotFoundPage; 