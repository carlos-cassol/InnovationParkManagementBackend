import React, { useState, useEffect } from 'react';
import {
  Box,
  Flex,
  VStack,
  HStack,
  Text,
  Button,
  IconButton,
  useDisclosure,
  useToast,
  Badge,
  Input,
  Textarea,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  FormControl,
  FormLabel,
  Select,
  Switch,
  Tooltip,
  Portal
} from '@chakra-ui/react';
import { AddIcon, EditIcon, DeleteIcon } from '@chakra-ui/icons';
import { DndContext, closestCenter, DragOverlay, useSensor, useSensors, PointerSensor, TouchSensor } from '@dnd-kit/core';
import { SortableContext, verticalListSortingStrategy, horizontalListSortingStrategy } from '@dnd-kit/sortable';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { getClients, createCard, updateCard, moveCardWithIndex, reorderColumn, convertIncubationPlanFromBackend } from '../services/api';

// Componente Card Sortable simplificado
const SortableCard = ({ card, onClick, onEdit, onDelete }) => {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({
    id: card.id,
    data: {
      type: 'card',
      card: card
    }
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  const getIncubationPlanText = (plan) => {
    // Converter do valor do backend para texto legível
    const planValue = typeof plan === 'number' ? convertIncubationPlanFromBackend(plan) : plan;
    switch (planValue) {
      case 'Start':
        return 'Início';
      case 'Grow':
        return 'Crescimento';
      default:
        return planValue;
    }
  };

  const getBadgeColor = (plan) => {
    // Converter do valor do backend para cor
    const planValue = typeof plan === 'number' ? convertIncubationPlanFromBackend(plan) : plan;
    switch (planValue) {
      case 'Start':
        return 'green';
      case 'Grow':
        return 'blue';
      default:
        return 'gray';
    }
  };

  return (
    <Box
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      bg="white"
      p={3}
      borderRadius="md"
      boxShadow="sm"
      cursor="grab"
      _hover={{ boxShadow: "md" }}
      transition="all 0.2s"
      border="1px solid"
      borderColor="gray.200"
      mb={2}
      position="relative"
    >
      <Box onClick={onClick} cursor="pointer">
        <Text fontWeight="bold" mb={1} fontSize="sm">{card.name}</Text>
        
        {card.description && (
          <Text fontSize="xs" color="gray.600" mb={2} noOfLines={2}>
            {card.description}
          </Text>
        )}

        {card.responsible && (
          <Text fontSize="xs" color="blue.600" mb={1}>
            <strong>Responsável:</strong> {card.responsible}
          </Text>
        )}

        <Flex justify="space-between" align="center" mb={2}>
          <Badge colorScheme={getBadgeColor(card.incubationPlan)} size="sm">
            {getIncubationPlanText(card.incubationPlan)}
          </Badge>
          {card.client && (
            <Text fontSize="xs" color="gray.500" noOfLines={1}>
              {card.client.name}
            </Text>
          )}
        </Flex>

        {card.incubatorType && (
          <Text fontSize="xs" color="purple.600" mb={1}>
            <strong>Tipo:</strong> {card.incubatorType}
          </Text>
        )}

        {card.incubationStatus && (
          <Text fontSize="xs" color="orange.600" mb={1}>
            <strong>Status:</strong> {card.incubationStatus}
          </Text>
        )}

        {card.technologyPlatform && (
          <Text fontSize="xs" color="teal.600" mb={1}>
            <strong>Plataforma:</strong> {card.technologyPlatform}
          </Text>
        )}

        <Flex gap={1} flexWrap="wrap">
          {card.isPaid && (
            <Badge colorScheme="green" size="sm">
              Pago
            </Badge>
          )}
          {card.files && card.files.length > 0 && (
            <Badge colorScheme="blue" size="sm">
              {card.files.length} arquivo{card.files.length > 1 ? 's' : ''}
            </Badge>
          )}
        </Flex>
      </Box>
      
      <HStack 
        position="absolute" 
        top={2} 
        right={2} 
        spacing={1}
        opacity={0}
        _groupHover={{ opacity: 1 }}
        transition="opacity 0.2s"
      >
        <IconButton
          size="xs"
          icon={<EditIcon />}
          onClick={(e) => {
            e.stopPropagation();
            onEdit(card);
          }}
          colorScheme="blue"
          variant="ghost"
        />
        <IconButton
          size="xs"
          icon={<DeleteIcon />}
          onClick={(e) => {
            e.stopPropagation();
            onDelete(card);
          }}
          colorScheme="red"
          variant="ghost"
        />
      </HStack>
    </Box>
  );
};

// Componente Coluna Sortable simplificado
const SortableColumn = ({ column, onCardClick, onAddCard, onEditCard, onDeleteCard, children }) => {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({
    id: column.id,
    data: {
      type: 'column',
      column: column
    }
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <Box
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      minW="320px"
      bg="gray.50"
      borderRadius="md"
      p={4}
      boxShadow="md"
      border="2px dashed transparent"
      _hover={{ borderColor: "blue.200" }}
      cursor="grab"
    >
      <Flex justify="space-between" align="center" mb={3}>
        <Text fontWeight="bold" fontSize="lg">{column.name}</Text>
        <IconButton
          size="sm"
          icon={<AddIcon />}
          onClick={onAddCard}
          colorScheme="teal"
          variant="ghost"
        />
      </Flex>
      <VStack spacing={2} align="stretch">
        {children}
      </VStack>
    </Box>
  );
};

// Modal para criar/editar card
const CardModal = ({ isOpen, onClose, card, columnId, onSave }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [responsible, setResponsible] = useState('');
  const [clientId, setClientId] = useState('');
  const [incubationPlan, setIncubationPlan] = useState('Start');
  const [isPaid, setIsPaid] = useState(false);
  const [incubatorType, setIncubatorType] = useState('');
  const [incubationStatus, setIncubationStatus] = useState('');
  const [technologyPlatform, setTechnologyPlatform] = useState('');
  const [freeDescription, setFreeDescription] = useState('');
  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(false);
  const toast = useToast();

  useEffect(() => {
    if (isOpen) {
      getClients()
        .then(setClients)
        .catch(error => {
          console.error('Erro ao carregar clientes:', error);
          toast({ title: 'Erro ao carregar clientes', status: 'error', duration: 3000 });
        });
      
      if (card) {
        setName(card.name || '');
        setDescription(card.description || '');
        setResponsible(card.responsible || '');
        setClientId(card.clientId || '');
        // Converter do valor do backend para o frontend
        const planValue = typeof card.incubationPlan === 'number' 
          ? convertIncubationPlanFromBackend(card.incubationPlan) 
          : card.incubationPlan || 'Start';
        setIncubationPlan(planValue);
        setIsPaid(card.isPaid || false);
        setIncubatorType(card.incubatorType || '');
        setIncubationStatus(card.incubationStatus || '');
        setTechnologyPlatform(card.technologyPlatform || '');
        setFreeDescription(card.freeDescription || '');
      } else {
        setName('');
        setDescription('');
        setResponsible('');
        setClientId('');
        setIncubationPlan('Start');
        setIsPaid(false);
        setIncubatorType('');
        setIncubationStatus('');
        setTechnologyPlatform('');
        setFreeDescription('');
      }
    }
  }, [isOpen, card, toast]);

  const handleSave = async () => {
    if (!name || !clientId) {
      toast({ title: 'Nome e cliente são obrigatórios', status: 'warning', duration: 3000 });
      return;
    }
    
    setLoading(true);
    try {
      const cardData = {
        name,
        description,
        responsible,
        clientId,
        incubationPlan,
        isPaid,
        incubatorType,
        incubationStatus,
        technologyPlatform,
        freeDescription
      };

      if (card) {
        await updateCard(card.id, { ...cardData, columnId: card.columnId });
        toast({ title: 'Card atualizado!', status: 'success', duration: 2000 });
      } else {
        await createCard({ ...cardData, columnId });
        toast({ title: 'Card criado!', status: 'success', duration: 2000 });
      }
      onSave();
      onClose();
    } catch (error) {
      console.error('Error saving card:', error);
      toast({ 
        title: 'Erro ao salvar card', 
        description: error.response?.data?.message || error.message,
        status: 'error', 
        duration: 5000 
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" scrollBehavior="inside">
      <ModalOverlay />
      <ModalContent maxW="800px">
        <ModalHeader>{card ? 'Editar Card' : 'Criar Card'}</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4} align="stretch">
            <FormControl>
              <FormLabel>Nome do Card *</FormLabel>
              <Input 
                value={name} 
                onChange={e => setName(e.target.value)}
                placeholder="Digite o nome do card"
              />
            </FormControl>
            
            <FormControl>
              <FormLabel>Descrição</FormLabel>
              <Textarea 
                value={description} 
                onChange={e => setDescription(e.target.value)}
                placeholder="Digite a descrição do card"
                rows={3}
              />
            </FormControl>

            <FormControl>
              <FormLabel>Responsável</FormLabel>
              <Input 
                value={responsible} 
                onChange={e => setResponsible(e.target.value)}
                placeholder="Digite o responsável da atividade"
              />
            </FormControl>

            <FormControl>
              <FormLabel>Cliente *</FormLabel>
              <Select placeholder="Selecione um cliente" value={clientId} onChange={e => setClientId(e.target.value)}>
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
              <Input 
                value={incubatorType} 
                onChange={e => setIncubatorType(e.target.value)}
                placeholder="Digite o tipo de incubadora"
              />
            </FormControl>

            <FormControl>
              <FormLabel>Status da Incubação</FormLabel>
              <Input 
                value={incubationStatus} 
                onChange={e => setIncubationStatus(e.target.value)}
                placeholder="Digite o status da incubação"
              />
            </FormControl>

            <FormControl>
              <FormLabel>Plataforma de Tecnologia</FormLabel>
              <Input 
                value={technologyPlatform} 
                onChange={e => setTechnologyPlatform(e.target.value)}
                placeholder="Digite a plataforma de tecnologia"
              />
            </FormControl>

            <FormControl>
              <FormLabel>Descrição Livre</FormLabel>
              <Textarea 
                value={freeDescription} 
                onChange={e => setFreeDescription(e.target.value)}
                placeholder="Digite uma descrição livre"
                rows={4}
              />
            </FormControl>

            <FormControl display="flex" alignItems="center">
              <FormLabel mb="0">Pago?</FormLabel>
              <Switch isChecked={isPaid} onChange={e => setIsPaid(e.target.checked)} />
            </FormControl>
          </VStack>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="teal" mr={3} onClick={handleSave} isLoading={loading} isDisabled={!name || !clientId}>
            {card ? 'Salvar' : 'Criar'}
          </Button>
          <Button onClick={onClose}>Cancelar</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

// Componente principal Kanban simplificado
const KanbanBoard = ({ workArea, onRefresh }) => {
  const [activeId, setActiveId] = useState(null);
  const [selectedCard, setSelectedCard] = useState(null);
  const [selectedColumn, setSelectedColumn] = useState(null);
  const { isOpen: isCardModalOpen, onOpen: onCardModalOpen, onClose: onCardModalClose } = useDisclosure();
  const toast = useToast();

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    }),
    useSensor(TouchSensor, {
      activationConstraint: {
        delay: 250,
        tolerance: 5,
      },
    })
  );

  const handleDragStart = (event) => {
    setActiveId(event.active.id);
  };

  const handleDragEnd = async (event) => {
    const { active, over } = event;
    setActiveId(null);

    if (!active || !over) return;

    const activeId = active.id;
    const overId = over.id;

    console.log('Drag end:', {
      activeId,
      overId,
      activeData: active.data?.current,
      overData: over.data?.current
    });

    const isCard = active.data?.current?.type === 'card';
    const isColumn = over.data?.current?.type === 'column';

    // Verificar se é uma coluna sendo movida
    const isColumnDragging = active.data?.current?.type === 'column';
    const isColumnTarget = over.data?.current?.type === 'column';

    if (isCard && isColumn) {
      try {
        const card = active.data.current.card;
        const targetColumn = over.data.current.column;
        
        const targetColumnCards = workArea.columns.find(col => col.id === targetColumn.id)?.cards || [];
        const newIndex = targetColumnCards.length;

        await moveCardWithIndex(activeId, overId, newIndex);
        toast({ title: 'Card movido!', status: 'success', duration: 2000 });
        onRefresh();
      } catch (error) {
        console.error('Erro ao mover card:', error);
        toast({ 
          title: 'Erro ao mover card', 
          description: error.response?.data?.message || error.message,
          status: 'error', 
          duration: 3000 
        });
      }
    } else if (isColumnDragging && isColumnTarget) {
      try {
        // Encontrar as posições das colunas
        const oldIndex = workArea.columns.findIndex(col => col.id === activeId);
        const newIndex = workArea.columns.findIndex(col => col.id === overId);
        
        console.log('Reordenação de coluna:', {
          activeId,
          overId,
          oldIndex,
          newIndex,
          columns: workArea.columns.map(c => ({ id: c.id, name: c.name, order: c.order }))
        });
        
        if (oldIndex !== newIndex && oldIndex !== -1 && newIndex !== -1) {
          // Enviar a nova posição (índice) para o backend
          await reorderColumn(activeId, newIndex);
          toast({ title: 'Coluna reordenada!', status: 'success', duration: 2000 });
          onRefresh();
        } else {
          console.log('Reordenação ignorada - posições iguais ou inválidas');
        }
      } catch (error) {
        console.error('Erro ao reordenar coluna:', error);
        toast({ 
          title: 'Erro ao reordenar coluna', 
          description: error.response?.data?.message || error.message,
          status: 'error', 
          duration: 3000 
        });
      }
    } else {
      console.log('Drag ignorado - não é card para coluna nem coluna para coluna');
    }
  };

  const handleCardClick = (card) => {
    setSelectedCard(card);
    onCardModalOpen();
  };

  const handleCardEdit = (card) => {
    setSelectedCard(card);
    onCardModalOpen();
  };

  const handleCardDelete = async (card) => {
    toast({ title: 'Funcionalidade de exclusão em desenvolvimento', status: 'info', duration: 2000 });
  };

  const handleAddCard = (column) => {
    setSelectedCard(null);
    setSelectedColumn(column);
    onCardModalOpen();
  };

  const handleCardSave = () => {
    onRefresh();
  };

  const activeCard = activeId ? workArea.columns.flatMap(col => col.cards).find(card => card.id === activeId) : null;
  const activeColumn = activeId ? workArea.columns.find(col => col.id === activeId) : null;

  return (
    <Box p={4}>
      <DndContext 
        sensors={sensors}
        collisionDetection={closestCenter} 
        onDragStart={handleDragStart}
        onDragEnd={handleDragEnd}
      >
        <Flex gap={4} overflowX="auto">
          <SortableContext items={workArea.columns.map(col => col.id)} strategy={horizontalListSortingStrategy}>
            {workArea.columns.map(column => (
              <SortableColumn
                key={column.id}
                column={column}
                onCardClick={handleCardClick}
                onAddCard={() => handleAddCard(column)}
                onEditCard={handleCardEdit}
                onDeleteCard={handleCardDelete}
              >
                <SortableContext items={column.cards.map(card => card.id)} strategy={verticalListSortingStrategy}>
                  {column.cards.map(card => (
                    <SortableCard 
                      key={card.id} 
                      card={card} 
                      onClick={() => handleCardClick(card)}
                      onEdit={handleCardEdit}
                      onDelete={handleCardDelete}
                    />
                  ))}
                </SortableContext>
              </SortableColumn>
            ))}
          </SortableContext>
        </Flex>
        
        <DragOverlay>
          {activeCard ? (
            <SortableCard 
              card={activeCard} 
              onClick={() => {}}
              onEdit={() => {}}
              onDelete={() => {}}
            />
          ) : activeColumn ? (
            <SortableColumn
              column={activeColumn}
              onCardClick={() => {}}
              onAddCard={() => {}}
              onEditCard={() => {}}
              onDeleteCard={() => {}}
            >
              {activeColumn.cards.map(card => (
                <SortableCard 
                  key={card.id} 
                  card={card} 
                  onClick={() => {}}
                  onEdit={() => {}}
                  onDelete={() => {}}
                />
              ))}
            </SortableColumn>
          ) : null}
        </DragOverlay>
      </DndContext>

      <CardModal
        isOpen={isCardModalOpen}
        onClose={onCardModalClose}
        card={selectedCard}
        columnId={selectedColumn?.id}
        onSave={handleCardSave}
      />
    </Box>
  );
};

export default KanbanBoard; 