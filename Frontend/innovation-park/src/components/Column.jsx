import React, { useState } from 'react';
import { Box, Heading, Button, VStack, useDisclosure } from '@chakra-ui/react';
import { useDroppable } from '@dnd-kit/core';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable';
import CardItem from './CardItem';
import CreateCardModal from './CreateCardModal';
import CardDetailsModal from './CardDetailsModal';

const Column = ({ column, onCardChange }) => {
  const { isOpen: isCreateOpen, onOpen: onCreateOpen, onClose: onCreateClose } = useDisclosure();
  const { isOpen: isDetailsOpen, onOpen: onDetailsOpen, onClose: onDetailsClose } = useDisclosure();
  const [selectedCard, setSelectedCard] = useState(null);

  const {
    attributes,
    listeners,
    setNodeRef: setSortableNodeRef,
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

  const { setNodeRef: setDroppableNodeRef } = useDroppable({
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

  const handleCardClick = (card) => {
    setSelectedCard(card);
    onDetailsOpen();
  };

  return (
    <Box 
      ref={(node) => {
        setSortableNodeRef(node);
        setDroppableNodeRef(node);
      }}
      style={style}
      {...attributes}
      {...listeners}
      minW="300px" 
      bg="gray.50" 
      borderRadius="md" 
      p={4} 
      boxShadow="md"
      minH="400px"
      border="2px dashed transparent"
      _hover={{ borderColor: "blue.200" }}
      cursor="grab"
    >
      <Heading size="md" mb={2}>{column.name}</Heading>
      <Button size="sm" colorScheme="teal" mb={2} onClick={onCreateOpen}>Adicionar Card</Button>
      <SortableContext items={column.cards.map(card => card.id)} strategy={verticalListSortingStrategy}>
        <VStack spacing={3} align="stretch">
          {column.cards.map(card => (
            <CardItem key={card.id} card={card} onClick={() => handleCardClick(card)} onCardChange={onCardChange} />
          ))}
        </VStack>
      </SortableContext>
      <CreateCardModal isOpen={isCreateOpen} onClose={onCreateClose} columnId={column.id} onCreated={onCardChange} />
      <CardDetailsModal isOpen={isDetailsOpen} onClose={onDetailsClose} card={selectedCard} onUpdated={onCardChange} />
    </Box>
  );
};

export default Column; 