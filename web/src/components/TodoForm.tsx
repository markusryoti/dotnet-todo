import { useState } from "react";
import type { Todo } from "../types";
import { Button, TextField } from "@mui/material";

export default function TodoForm({
  addTodo,
}: {
  addTodo: (newTodo: Todo) => void;
}) {
  const [title, setTitle] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const form = e.target as HTMLFormElement;
    const input = form.elements[0] as HTMLInputElement;
    const title = input.value.trim();

    if (title) {
      const newTodo: Todo = {
        id: Date.now(),
        title,
        completed: false,
        createdAt: new Date().toISOString(),
      };

      addTodo(newTodo);
      setTitle("");
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div style={{ display: "flex", gap: "8px", alignItems: "center" }}>
        <TextField
          id="outlined-basic"
          label="Todo Name"
          variant="outlined"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          size="small"
        />
        <Button variant="contained" type="submit">
          Add Todo
        </Button>
      </div>
    </form>
  );
}
