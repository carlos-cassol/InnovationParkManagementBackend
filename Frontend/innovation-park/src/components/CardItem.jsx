import React from 'react';
import { Box, Text, Badge, Flex } from '@chakra-ui/react';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

const CardItem = ({ card, onClick, onCardChange }) => {
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
    switch (plan) {
      case 'BASIC': return 'Básico';
      case 'ADVANCED': return 'Avançado';
      case 'PREMIUM': return 'Premium';
      default: return plan;
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
      onClick={onClick}
      border="1px solid"
      borderColor="gray.200"
    >
      <Text fontWeight="bold" mb={1}>{card.name}</Text>
      <Text fontSize="sm" color="gray.600" mb={2} noOfLines={2}>
        {card.description}
      </Text>
      <Flex justify="space-between" align="center">
        <Badge colorScheme={card.incubationPlan === 'BASIC' ? 'green' : card.incubationPlan === 'ADVANCED' ? 'blue' : 'purple'}>
          {getIncubationPlanText(card.incubationPlan)}
        </Badge>
        {card.client && (
          <Text fontSize="xs" color="gray.500">
            {card.client.name}
          </Text>
        )}
      </Flex>
    </Box>
  );
};

export default CardItem; 