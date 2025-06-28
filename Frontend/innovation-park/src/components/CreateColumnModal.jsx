import React, { useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody, ModalCloseButton, Button, FormControl, FormLabel, Input, useToast } from '@chakra-ui/react';
import { createColumn } from '../services/api';

const CreateColumnModal = ({ isOpen, onClose, workAreaId, onCreated }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const toast = useToast();

  const handleCreate = async () => {
    if (!name) return;
    try {
      await createColumn({ name, description, workAreaId });
      toast({ title: 'Coluna criada!', status: 'success', duration: 2000 });
      setName('');
      setDescription('');
      onClose();
      onCreated();
    } catch (error) {
      console.error('Error creating column:', error);
      toast({ 
        title: 'Erro ao criar coluna', 
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
        <ModalHeader>Criar Coluna</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}>
            <FormLabel>Nome</FormLabel>
            <Input value={name} onChange={e => setName(e.target.value)} />
          </FormControl>
          <FormControl>
            <FormLabel>Descrição</FormLabel>
            <Input value={description} onChange={e => setDescription(e.target.value)} />
          </FormControl>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="teal" mr={3} onClick={handleCreate} isDisabled={!name}>
            Criar
          </Button>
          <Button onClick={onClose}>Cancelar</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default CreateColumnModal; 