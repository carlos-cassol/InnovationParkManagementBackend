import React from 'react';
import { Box, Heading, Text, Button } from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';

const WorkspaceCard = ({ workspace }) => {
  const navigate = useNavigate();
  return (
    <Box borderWidth="1px" borderRadius="lg" p={4} boxShadow="md">
      <Heading size="md" mb={2}>{workspace.name}</Heading>
      <Text mb={2}>{workspace.description}</Text>
      <Button colorScheme="teal" onClick={() => navigate(`/workspace/${workspace.id}`)}>
        Abrir Quadro
      </Button>
    </Box>
  );
};

export default WorkspaceCard; 