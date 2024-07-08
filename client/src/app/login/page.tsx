"use client";
import { useState, useEffect } from "react";
import { redirect, useRouter } from "next/navigation";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { useCookies } from "react-cookie";
import { jwtDecode } from "jwt-decode";
import { ICookieInfo, IUser } from "@/types";

export default function Login() {
  const router = useRouter();
  const [cookies, setCookie, removeCookie] = useCookies(["test-cookies"]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [usernameInput, setUsername] = useState("");
  const [passwordInput, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const authUser = async () => {
    if (usernameInput == "") setErrorMessage("Вы не ввели имя пользователя!");
    else if (passwordInput == "") setErrorMessage("Вы не ввели пароль!");
    else {
      const authInfo = { username: usernameInput, password: passwordInput };
      const response = await fetch("http://localhost:5000/api/user/auth", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(authInfo),
      });
      const data = await response.json();
      if (data.code !== undefined) {
        setErrorMessage(data.description);
        return;
      }
      setLocalStorage(data);
      setUsername("");
      setPassword("");
    }
  };

  const setLocalStorage = (user: IUser) => {
    localStorage.setItem("user", JSON.stringify(user));
    if (cookies["test-cookies"]) {
      const token = cookies["test-cookies"];
      const decoded = jwtDecode(token) as ICookieInfo;
      localStorage.setItem("token", JSON.stringify(decoded));
    }
  };

  useEffect(() => {
    if (cookies["test-cookies"]) {
      const token = cookies["test-cookies"];
      const decoded = jwtDecode(token) as ICookieInfo;
      if (Date.now() >= decoded.exp * 1000) {
        removeCookie("test-cookies", { path: "/" });
        localStorage.removeItem("userId");
        localStorage.removeItem("token");
        console.log("cookie removed");
        setIsLoading(false);
        return;
      }
      localStorage.setItem("token", JSON.stringify(decoded));
      localStorage.setItem("userId", decoded.userId);
      console.log("redirected");
      redirect("/main");
    }
    setIsLoading(false);
  }, [cookies]);

  const signUp = () => {
    router.push("/signup");
  };

  if (isLoading) return null;

  return (
    <>
      <div className="mx-auto gap-y-2 flex flex-col justify-center items-center h-[100vh] w-[20%]">
        <Input
          onChange={(e) => {
            setUsername(e.target.value);
            setErrorMessage("");
          }}
          required
          id="username"
          name="username"
          placeholder="Имя пользователя"
        />
        <Input
          onChange={(e) => {
            setPassword(e.target.value);
            setErrorMessage("");
          }}
          required
          id="password"
          name="password"
          type="password"
          placeholder="Пароль"
        />
        {errorMessage !== "" && (
          <Label className="text-red-600 cursor-pointer text-[0.875rem] overflow-hiden text-ellipsis">
            {errorMessage}
          </Label>
        )}
        <Button
          className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300"
          onClick={() => authUser()}
        >
          Войти
        </Button>
        <Button
          className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300"
          onClick={() => signUp()}
        >
          Зарегистрироваться
        </Button>
      </div>
    </>
  );
}
