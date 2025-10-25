import TodoForm from "./components/TodoForm";
import TodoList from "./components/TodoList";
import { useState } from "react";
import type { Todo } from "./types";

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);

  const addTodo = (newTodo: Todo) => {
    setTodos((prevTodos) => [...prevTodos, newTodo]);
  };

  const toggleDone = (id: number) => {
    const updated = todos.map((t) => {
      if (t.id === id) {
        return {
          ...t,
          completed: !t.completed,
        };
      }

      return t;
    });

    setTodos(updated);
  };

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        paddingTop: "24px",
      }}
    >
      <TodoForm addTodo={addTodo} />
      <TodoList todos={todos} toggleDone={toggleDone} />
    </div>
  );
}

export default App;
