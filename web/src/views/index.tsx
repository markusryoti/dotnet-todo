import TodoForm from "../components/TodoForm";
import TodoList from "../components/TodoList";
import type { Todo } from "../types";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { addTodo, deleteTodo, getTodos, updateTodo } from "../query";
import { useAuth } from "../hooks/useAuth";
import { useNavigate } from "react-router";

function Index() {
  const { getToken } = useAuth();
  const navigate = useNavigate();

  const { data: todos, isPending: loading } = useQuery({
    queryKey: ["todos"],
    queryFn: async () => {
      const token = await getToken();
      if (!token) {
        await navigate("/login");
        return [];
      }

      return await getTodos(token);
    },
  });

  const queryClient = useQueryClient();

  const addMutation = useMutation({
    mutationFn: addTodo,
  });

  const updateMutation = useMutation({
    mutationFn: updateTodo,
  });

  const deleteMutation = useMutation({
    mutationFn: deleteTodo,
  });

  const submitTodo = async (newTodo: Todo) => {
    const token = await getToken();
    if (!token) {
      await navigate("/login");
      return;
    }

    addMutation.mutate(
      { data: newTodo, token },
      {
        onSuccess: async () => {
          await queryClient.invalidateQueries({ queryKey: ["todos"] });
        },
      }
    );
  };

  const removeTodo = async (id: number) => {
    const token = await getToken();
    if (!token) {
      await navigate("/login");
      return;
    }

    deleteMutation.mutate(
      { data: { id }, token },
      {
        onSuccess: async () => {
          await queryClient.invalidateQueries({ queryKey: ["todos"] });
        },
      }
    );
  };

  const changeTodo = async (patchedTodo: Todo) => {
    const token = await getToken();
    if (!token) {
      await navigate("/login");
      return;
    }

    updateMutation.mutate(
      { data: patchedTodo, token },
      {
        onSuccess: async () => {
          await queryClient.invalidateQueries({ queryKey: ["todos"] });
        },
      }
    );
  };

  return (
    <div className="container">
      <div className="todo-container">
        <TodoForm addTodo={submitTodo} />
        <TodoList
          todos={todos}
          updateTodo={changeTodo}
          deleteTodo={removeTodo}
          loading={loading}
        />
      </div>
    </div>
  );
}

export default Index;
