import type { Todo } from "./types";

const ApiUrl = import.meta.env.VITE_PUBLIC_API_URL;

export async function getTodos(token: string) {
  const response = await fetch(`${ApiUrl}/todos`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  return (await response.json()) as Todo[];
}

export async function addTodo(request: {
  data: {
    title: string;
    isComplete: boolean;
  };
  token: string;
}) {
  const response = await fetch(`${ApiUrl}/todos`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${request.token}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request.data),
  });
  if (!response.ok) {
    throw new Error("Error adding todo");
  }
}

export async function updateTodo(request: {
  data: {
    id: number;
    title: string;
    isComplete: boolean;
  };
  token: string;
}) {
  const response = await fetch(`${ApiUrl}/todos/${request.data.id}`, {
    method: "PATCH",
    headers: {
      Authorization: `Bearer ${request.token}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request.data),
  });
  if (!response.ok) {
    throw new Error("Error adding todo");
  }
}

export async function deleteTodo(request: {
  data: { id: number };
  token: string;
}) {
  const response = await fetch(`${ApiUrl}/todos/${request.data.id}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${request.token}`,
    },
  });
  if (!response.ok) {
    throw new Error("Error deleting todo");
  }
}
