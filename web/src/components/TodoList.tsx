import { Checkbox, List, ListItem, ListItemText } from "@mui/material";
import type { Todo } from "../types";

export default function TodoList({
  todos,
  toggleDone,
}: {
  todos: Todo[];
  toggleDone: (id: number) => void;
}) {
  return (
    <List sx={{ width: "100%", maxWidth: 360, bgcolor: "background.paper" }}>
      {todos.map((todo) => (
        <ListItem key={todo.id}>
          <ListItemText
            primary={todo.title}
            secondary={todo.createdAt}
            style={todo.completed ? { textDecoration: "line-through" } : {}}
          />
          <Checkbox
            edge="end"
            checked={todo.completed}
            onChange={() => toggleDone(todo.id)}
          />
        </ListItem>
      ))}
    </List>
  );
}
