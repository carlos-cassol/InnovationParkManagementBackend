import React, { useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody, ModalCloseButton, Button, FormControl, FormLabel, Input, Select, useToast } from '@chakra-ui/react';
import { createCard, getClients } from '../services/api';

const CreateCardModal = ({ isOpen, onClose, columnId, onCreated }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [clientId, setClientId] = useState('');
  const [incubationPlan, setIncubationPlan] = useState('BASIC');
  const [clients, setClients] = useState([]);
  const toast = useToast();

  React.useEffect(() => {
    if (isOpen) {
      getClients().then(setClients);
    }
  }, [isOpen]);

  const handleCreate = async () => {
    if (!name || !clientId) return;
    try {
      await createCard({ name, description, clientId, columnId, incubationPlan });
      toast({ title: 'Card criado!', status: 'success', duration: 2000 });
      setName('');
      setDescription('');
      setClientId('');
      setIncubationPlan('BASIC');
      onClose();
      onCreated();
    } catch (error) {
      console.error('Error creating card:', error);
      toast({ 
        title: 'Erro ao criar card', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Criar Card</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}>
            <FormLabel>Nome</FormLabel>
            <Input value={name} onChange={e => setName(e.target.value)} />
          </FormControl>
          <FormControl mb={3}>
            <FormLabel>Descrição</FormLabel>
            <Input value={description} onChange={e => setDescription(e.target.value)} />
          </FormControl>
          <FormControl mb={3}>
            <FormLabel>Cliente</FormLabel>
            <Select placeholder="Selecione um cliente" value={clientId} onChange={e => setClientId(e.target.value)}>
              {clients.map(client => (
                <option key={client.id} value={client.id}>{client.name}</option>
              ))}
            </Select>
          </FormControl>
          <FormControl>
            <FormLabel>Plano de Incubação</FormLabel>
            <Select value={incubationPlan} onChange={e => setIncubationPlan(e.target.value)}>
              <option value="BASIC">Básico</option>
              <option value="ADVANCED">Avançado</option>
              <option value="PREMIUM">Premium</option>
            </Select>
          </FormControl>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="teal" mr={3} onClick={handleCreate} isDisabled={!name || !clientId}>
            Criar
          </Button>
          <Button onClick={onClose}>Cancelar</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default CreateCardModal; 