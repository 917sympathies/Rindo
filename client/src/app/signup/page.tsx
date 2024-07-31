"use client";
import { useState } from "react";
import { useRouter } from "next/navigation"
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { SignUpUser } from "@/requests";

export default function SignUp() {
  const router = useRouter();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const signUp = async () => {
      const response = await SignUpUser(username, password, email, firstName, lastName);
      const data = await response.json();
      if(data.errors === undefined && data.description === undefined){
          router.push("/login")
      }
      else if(data.errors === undefined){
        setErrorMessage(data.description)
      }
      else{
        if (data.errors.Username !== undefined) setErrorMessage(data.errors.Username);
        else if (data.errors.Password !== undefined) setErrorMessage(data.errors.Password);
        else if (data.errors.Email !== undefined) setErrorMessage(data.errors.Email);
        else if (data.errors.FirstName !== undefined) setErrorMessage(data.errors.FirstName);
        else if (data.errors.LastName !== undefined) setErrorMessage(data.errors.LastName);
      }
  };

  return (
    <div className="flex flex-col justify-center gap-[0.5rem] items-center h-[100vh] w-[20%] m-auto">
      <Input
        required
        placeholder="Имя пользователя"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      ></Input>
      <Input
        required
        placeholder="Пароль"
        value={password}
        type="password"
        onChange={(e) => setPassword(e.target.value)}
      ></Input>
      <Input
        required
        placeholder="E-mail"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      ></Input>
      <Input
        required
        placeholder="Имя"
        value={firstName}
        onChange={(e) => setFirstName(e.target.value)}
      ></Input>
      <Input
        required
        placeholder="Фамилия"
        value={lastName}
        onChange={(e) => setLastName(e.target.value)}
      ></Input>
      {errorMessage !== "" && (
        <Label className="text-red-600 cursor-pointer text-[0.875rem] overflow-hiden text-ellipsis">
          {errorMessage}
        </Label>
      )}
      <Button className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300" onClick={() => signUp()}>Зарегистрироваться</Button>
    </div>
  );
}
