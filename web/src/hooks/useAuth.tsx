import { createContext, useContext, useState } from "react";

const ApiUrl = import.meta.env.VITE_PUBLIC_API_URL;

interface IAuthContext {
  getToken: () => Promise<string | undefined>;
  login: (email: string, password: string) => Promise<void>;
}

interface LoginResponse {
  accessToken: string;
}

const AuthContext = createContext<IAuthContext>({} as IAuthContext);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [accessToken, setAccessToken] = useState<string>();

  const getToken = async () => {
    if (accessToken) {
      return accessToken;
    }

    const fromStorage = localStorage.getItem("accessToken");
    return fromStorage ? fromStorage : undefined;
  };

  const login = async (email: string, password: string) => {
    console.log("logging in");

    const res = await fetch(`${ApiUrl}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        username: email,
        password,
      }),
    });

    if (!res.ok) {
      throw new Error("Login failed");
    }

    const { accessToken } = (await res.json()) as LoginResponse;

    setAccessToken(accessToken);
    localStorage.setItem("accessToken", accessToken);
  };

  return <AuthContext value={{ getToken, login }}>{children}</AuthContext>;
};

export function useAuth() {
  const ctx = useContext(AuthContext);
  return ctx;
}
