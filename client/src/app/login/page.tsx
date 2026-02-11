"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { TokenDto } from "@/types";
import { authUser } from "@/requests";


export default function Login() {
    const router = useRouter();
    const [usernameInput, setUsername] = useState("");
    const [passwordInput, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    const auth = async () => {
        if (usernameInput == "" || passwordInput == "") {
            setErrorMessage("Нужно заполнить все поля");
            return;
        }
        authUser(usernameInput, passwordInput)
            .then(response => {
                setLocalStorage(response.data);
                setUsername("");
                setPassword("");
                router.push("/main")
            })
            .catch(error => {
                if (error.response.data) {
                    setErrorMessage(error.response.data);
                    return;
                }
            })
    };

    const setLocalStorage = (tokenDto: TokenDto) => {
        localStorage.setItem("user", JSON.stringify(tokenDto.user));
        localStorage.setItem("userId", tokenDto.user.id);
        localStorage.setItem("token", tokenDto.token);
    };

    const signUp = () => {
        router.push("/signup");
    };

    return (
        <form action={auth}>
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
                <Button type="submit" className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300">
                    Войти
                </Button>
                <Button className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300" onClick={() => signUp()}>
                    Регистрация
                </Button>
            </div>
        </form>
    );
}
