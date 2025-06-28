import React, { useEffect, useState } from 'react';
import {
  Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody, ModalCloseButton,
  Button, FormControl, FormLabel, Input, Textarea, Select, Switch, VStack, Box, Text, Badge, useToast,
  HStack, IconButton, Divider, Alert, AlertIcon
} from '@chakra-ui/react';
import { DownloadIcon, DeleteIcon, AttachmentIcon } from '@chakra-ui/icons';
import { getClients, updateCard, uploadCardFile, getCardFiles, convertIncubationPlanFromBackend } from '../services/api';

const CardDetailsModal = ({ isOpen, onClose, card, onUpdated }) => {
  const [name, setName] = useState(card?.name || '');
  const [description, setDescription] = useState(card?.description || '');
  const [responsible, setResponsible] = useState(card?.responsible || '');
  const [clientId, setClientId] = useState(card?.clientId || '');
  const [isPaid, setIsPaid] = useState(card?.isPaid || false);
  const [incubationPlan, setIncubationPlan] = useState(card?.incubationPlan || 'Start');
  const [incubatorType, setIncubatorType] = useState(card?.incubatorType || '');
  const [incubationStatus, setIncubationStatus] = useState(card?.incubationStatus || '');
  const [technologyPlatform, setTechnologyPlatform] = useState(card?.technologyPlatform || '');
  const [freeDescription, setFreeDescription] = useState(card?.freeDescription || '');
  const [clients, setClients] = useState([]);
  const [files, setFiles] = useState([]);
  const [file, setFile] = useState(null);
  const [paymentReceipt, setPaymentReceipt] = useState(null);
  const [uploading, setUploading] = useState(false);
  const toast = useToast();

  useEffect(() => {
    if (isOpen) {
      getClients().then(setClients);
      if (card) {
        getCardFiles(card.id).then(setFiles);
        setName(card?.name || '');
        setDescription(card?.description || '');
        setResponsible(card?.responsible || '');
        setClientId(card?.clientId || '');
        setIsPaid(card?.isPaid || false);
        setIncubatorType(card?.incubatorType || '');
        setIncubationStatus(card?.incubationStatus || '');
        setTechnologyPlatform(card?.technologyPlatform || '');
        setFreeDescription(card?.freeDescription || '');
        // Converter do valor do backend para o frontend
        const planValue = typeof card?.incubationPlan === 'number' 
          ? convertIncubationPlanFromBackend(card.incubationPlan) 
          : card?.incubationPlan || 'Start';
        setIncubationPlan(planValue);
      }
    }
  }, [isOpen, card]);

  const handleUpdate = async () => {
    try {
      const cardData = {
        name,
        description,
        responsible,
        clientId,
        isPaid,
        incubationPlan,
        incubatorType,
        incubationStatus,
        technologyPlatform,
        freeDescription,
        columnId: card.columnId
      };
      
      await updateCard(card.id, cardData);
      toast({ title: 'Card atualizado!', status: 'success', duration: 2000 });
      onUpdated();
      onClose();
    } catch (error) {
      console.error('Error updating card:', error);
      toast({ 
        title: 'Erro ao atualizar card', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    }
  };

  const handleUpload = async (isPaymentReceipt = false) => {
    const selectedFile = isPaymentReceipt ? paymentReceipt : file;
    if (!selectedFile) {
      toast({ title: 'Selecione um arquivo', status: 'warning', duration: 3000 });
      return;
    }

    console.log('Iniciando upload:', {
      fileName: selectedFile.name,
      fileSize: selectedFile.size,
      fileType: selectedFile.type,
      isPaymentReceipt,
      cardId: card.id
    });

    setUploading(true);
    try {
      const formData = new FormData();
      formData.append('file', selectedFile);
      formData.append('cardId', card.id);
      formData.append('isPaymentReceipt', isPaymentReceipt.toString());
      
      console.log('FormData criado:', {
        cardId: formData.get('cardId'),
        isPaymentReceipt: formData.get('isPaymentReceipt'),
        fileName: formData.get('file')?.name
      });
      
      const result = await uploadCardFile(formData);
      console.log('Upload bem-sucedido:', result);
      
      toast({ title: 'Arquivo enviado!', status: 'success', duration: 2000 });
      
      // Limpar campos
      if (isPaymentReceipt) {
        setPaymentReceipt(null);
      } else {
        setFile(null);
      }
      
      // Recarregar arquivos
      const updatedFiles = await getCardFiles(card.id);
      setFiles(updatedFiles);
    } catch (error) {
      console.error('Erro no upload:', error);
      toast({ 
        title: 'Erro ao enviar arquivo', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    } finally {
      setUploading(false);
    }
  };

  const handleDownload = (fileId, fileName) => {
    const link = document.createElement('a');
    link.href = `http://localhost:5297/api/CardFile/download/${fileId}`;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" scrollBehavior="inside">
      <ModalOverlay />
      <ModalContent maxW="900px">
        <ModalHeader>Detalhes do Card</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4} align="stretch">
            <FormControl>
              <FormLabel>Nome do Card</FormLabel>
              <Input value={name} onChange={e => setName(e.target.value)} />
            </FormControl>
            
            <FormControl>
              <FormLabel>Descrição</FormLabel>
              <Textarea value={description} onChange={e => setDescription(e.target.value)} />
            </FormControl>

            <FormControl>
              <FormLabel>Responsável</FormLabel>
              <Input value={responsible} onChange={e => setResponsible(e.target.value)} />
            </FormControl>

            <FormControl>
              <FormLabel>Cliente</FormLabel>
              <Select value={clientId} onChange={e => setClientId(e.target.value)}>
                {clients.map(client => (
                  <option key={client.id} value={client.id}>{client.name}</option>
                ))}
              </Select>
            </FormControl>

            <FormControl>
              <FormLabel>Plano de Incubação</FormLabel>
              <Select value={incubationPlan} onChange={e => setIncubationPlan(e.target.value)}>
                <option value="Start">Início</option>
                <option value="Grow">Crescimento</option>
              </Select>
            </FormControl>

            <FormControl>
              <FormLabel>Tipo de Incubadora</FormLabel>
              <Input value={incubatorType} onChange={e => setIncubatorType(e.target.value)} />
            </FormControl>

            <FormControl>
              <FormLabel>Status da Incubação</FormLabel>
              <Input value={incubationStatus} onChange={e => setIncubationStatus(e.target.value)} />
            </FormControl>

            <FormControl>
              <FormLabel>Plataforma de Tecnologia</FormLabel>
              <Input value={technologyPlatform} onChange={e => setTechnologyPlatform(e.target.value)} />
            </FormControl>

            <FormControl>
              <FormLabel>Descrição Livre</FormLabel>
              <Textarea value={freeDescription} onChange={e => setFreeDescription(e.target.value)} rows={4} />
            </FormControl>

            <FormControl display="flex" alignItems="center">
              <FormLabel mb="0">Pago?</FormLabel>
              <Switch isChecked={isPaid} onChange={e => setIsPaid(e.target.checked)} />
            </FormControl>

            <Divider />

            <Box>
              <Text fontWeight="bold" mb={3} fontSize="lg">
                <AttachmentIcon mr={2} />
                Arquivos Anexados
              </Text>
              
              {files.length > 0 ? (
                <VStack spacing={2} align="stretch">
                  {files.map(f => (
                    <Box key={f.id} p={3} borderWidth="1px" borderRadius="md" bg="gray.50">
                      <HStack justify="space-between">
                        <VStack align="start" spacing={1}>
                          <Text fontWeight="medium">{f.fileName}</Text>
                          <HStack spacing={2}>
                            <Badge colorScheme={f.isPaymentReceipt ? 'green' : 'blue'}>
                              {f.isPaymentReceipt ? 'Comprovante de Pagamento' : 'Anexo'}
                            </Badge>
                            <Text fontSize="xs" color="gray.500">
                              {new Date(f.uploadDate).toLocaleDateString()}
                            </Text>
                          </HStack>
                        </VStack>
                        <IconButton
                          size="sm"
                          icon={<DownloadIcon />}
                          onClick={() => handleDownload(f.id, f.fileName)}
                          colorScheme="blue"
                          variant="ghost"
                        />
                      </HStack>
                    </Box>
                  ))}
                </VStack>
              ) : (
                <Alert status="info">
                  <AlertIcon />
                  Nenhum arquivo anexado
                </Alert>
              )}

              <VStack spacing={3} mt={4}>
                <FormControl>
                  <FormLabel>Adicionar Arquivo</FormLabel>
                  <Input 
                    type="file" 
                    onChange={e => setFile(e.target.files[0])}
                    accept="*/*"
                  />
                  <Button 
                    size="sm" 
                    mt={2} 
                    colorScheme="teal" 
                    onClick={() => handleUpload(false)} 
                    isDisabled={!file}
                    isLoading={uploading}
                    leftIcon={<AttachmentIcon />}
                  >
                    Enviar Arquivo
                  </Button>
                </FormControl>

                <FormControl>
                  <FormLabel>Comprovante de Pagamento</FormLabel>
                  <Input 
                    type="file" 
                    onChange={e => setPaymentReceipt(e.target.files[0])}
                    accept="image/*,.pdf"
                  />
                  <Button 
                    size="sm" 
                    mt={2} 
                    colorScheme="green" 
                    onClick={() => handleUpload(true)} 
                    isDisabled={!paymentReceipt}
                    isLoading={uploading}
                    leftIcon={<DownloadIcon />}
                  >
                    Enviar Comprovante
                  </Button>
                </FormControl>
              </VStack>
            </Box>
          </VStack>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="teal" mr={3} onClick={handleUpdate}>
            Salvar
          </Button>
          <Button onClick={onClose}>Fechar</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default CardDetailsModal; 