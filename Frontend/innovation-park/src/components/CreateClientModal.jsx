import React, { useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody, ModalCloseButton, Button, FormControl, FormLabel, Input, useToast } from '@chakra-ui/react';
import { createClient } from '../services/api';

const CreateClientModal = ({ isOpen, onClose, onCreated }) => {
  const [name, setName] = useState('');
  const [cpfCnpj, setCpfCnpj] = useState('');
  const [contact, setContact] = useState('');
  const [address, setAddress] = useState('');
  const toast = useToast();

  const handleCreate = async () => {
    if (!name || !cpfCnpj || !contact || !address) {
      toast({ title: 'Todos os campos são obrigatórios', status: 'error', duration: 3000 });
      return;
    }

    try {
      await createClient({ name, cpfCnpj, contact, address });
      toast({ title: 'Cliente criado!', status: 'success', duration: 2000 });
      setName('');
      setCpfCnpj('');
      setContact('');
      setAddress('');
      onClose();
      onCreated();
    } catch (error) {
      console.error('Error creating client:', error);
      toast({ 
        title: 'Erro ao criar cliente', 
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
        <ModalHeader>Criar Cliente</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}>
            <FormLabel>Nome</FormLabel>
            <Input value={name} onChange={e => setName(e.target.value)} />
          </FormControl>
          <FormControl mb={3}>
            <FormLabel>CPF/CNPJ</FormLabel>
            <Input value={cpfCnpj} onChange={e => setCpfCnpj(e.target.value)} />
          </FormControl>
          <FormControl mb={3}>
            <FormLabel>Contato</FormLabel>
            <Input value={contact} onChange={e => setContact(e.target.value)} />
          </FormControl>
          <FormControl>
            <FormLabel>Endereço</FormLabel>
            <Input value={address} onChange={e => setAddress(e.target.value)} />
          </FormControl>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="teal" mr={3} onClick={handleCreate} isDisabled={!name || !cpfCnpj || !contact || !address}>
            Criar
          </Button>
          <Button onClick={onClose}>Cancelar</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default CreateClientModal; 