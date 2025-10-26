import {
  Checkbox,
  IconButton,
  List,
  ListItem,
  ListItemText,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import type { Todo } from "../types";

export default function TodoList({
  todos,
  updateTodo,
  deleteTodo,
  loading,
}: {
  todos: Todo[] | undefined;
  updateTodo: (id: number, updated: Todo) => void;
  deleteTodo: (id: number) => void;
  loading: boolean;
}) {
  if (loading) {
    return <p>Loading...</p>;
  }

  const sorted = todos?.sort((a, b) => a.id - b.id);

  return (
    <List sx={{ width: "100%", maxWidth: 360, bgcolor: "background.paper" }}>
      {sorted?.map((todo) => (
        <ListItem key={todo.id}>
          <ListItemText
            primary={todo.title}
            secondary={todo.createdAt}
            style={todo.isComplete ? { textDecoration: "line-through" } : {}}
          />
          <Checkbox
            edge="end"
            checked={todo.isComplete}
            onChange={() =>
              updateTodo(todo.id, { ...todo, isComplete: !todo.isComplete })
            }
          />
          <IconButton
            edge="end"
            aria-label="delete"
            onClick={() => deleteTodo(todo.id)}
          >
            <DeleteIcon />
          </IconButton>
        </ListItem>
      ))}
    </List>
  );
}
