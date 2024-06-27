'use client';
import { useState, useContext, useEffect } from "react";
import styles from "./styles.module.css";
import { redirect, useRouter } from "next/navigation";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { useCookies } from "react-cookie"
import { jwtDecode } from "jwt-decode";
import { ICookieInfo, IUser } from "@/types";


export default function Login() {
  const router = useRouter();
  const [cookies, setCookie, removeCookie] = useCookies(['test-cookies']);
  const [isAuth, setIsAuth] = useState(false);
  const [isLoading, setIsLoading] = useState<boolean>(true)
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
        credentials: 'include',
        body: JSON.stringify(authInfo),
      });
      const data = await response.json();
      if(data.code !== undefined){
        setErrorMessage(data.description);
        return;
      }
      setLocalStorage(data);
      setIsAuth(true);
      setUsername("");
      setPassword("");
    }
  };

  // if(cookies["test-cookies"]){
  //   const token = cookies["test-cookies"];
  //   const decoded = jwtDecode(token) as CookieInfo;
  //   // var date =new Date('1970-01-01T00:00:00Z'); 
  //   // console.log(date.setUTCSeconds(decoded.exp))
  //   console.log("exp > now: ")
  //   console.log(decoded.exp > new Date().getTime());
  //   if(decoded.exp > new Date().getTime()){
  //     removeCookie("test-cookies", {path:'/'});
  //     return;
  //   }

  //   setIsAuth(true);
  // }

  const setLocalStorage = (user : IUser) => {
    localStorage.setItem("user", JSON.stringify(user));
    if(cookies['test-cookies']){
      const token = cookies["test-cookies"];
      const decoded = jwtDecode(token) as ICookieInfo;
      localStorage.setItem("token",JSON.stringify(decoded));
    }
  }
  
  useEffect(() => {
    if(cookies['test-cookies']){
      const token = cookies["test-cookies"];
      const decoded = jwtDecode(token) as ICookieInfo;
      if(Date.now() >= decoded.exp * 1000){
        removeCookie("test-cookies", {path:'/'});
        localStorage.removeItem("userId");
        localStorage.removeItem("token");
        console.log("cookie removed")
        setIsAuth(false);
        setIsLoading(false);
        return;
      }
      // console.log(decoded.exp > new Date().getTime())
      // if(decoded.exp > new Date().getTime()){
      //   removeCookie("test-cookies", {path:'/'});
      //   console.log("cookie removed")
      //   setIsLoading(false);
      //   return;
      // }
      localStorage.setItem("token", JSON.stringify(decoded));
      setIsAuth(true);
      localStorage.setItem("userId", decoded.userId);
      console.log("redirected")
      redirect('/main')
    }
    setIsLoading(false);
  }, [cookies]);

  // useEffect(() => {
  //   const token = localStorage.getItem("token");
  //   if(!token){
  //     setIsLoading(false);
  //     return;
  //   }
  //   const exp = JSON.parse(token).exp;
  //   if(exp > new Date().getTime()){
  //     removeCookie("test-cookies", {path:'/'});
  //     setIsLoading(false);
  //     return;
  //   }
  //   setIsAuth(true);
  //   redirect('/main')
  // }, [])

  const signUp = () => {
    router.push("/signup");
  };

  if(isLoading) return null;

  return (
    <>
      {/* {isAuth && errorMessage === "" ? (
        redirect("/main")
      ) : ( */}
        <div
          className="mx-auto gap-y-2"
          style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            flexDirection: "column",
            height: "100vh",
            width: "20%",
          }}
        >
          {/* <svg
            xmlns="http://www.w3.org/2000/svg"
            width="100"
            height="100"
            viewBox="0 0 100 100"
            fill="none"
          >
            <rect width="36" height="36" fill="white" />
            <path
              d="M42.5 16H59L30 75H12L42.5 16Z"
              fill="#88FFD4"
              fill-opacity="0.7"
            />
            <path
              d="M54.6383 46.514C44.2979 45.3575 42 46.514 42 46.514L64.2128 92H78C78 92 64.9787 47.6704 54.6383 46.514Z"
              fill="#88FFD4"
              fill-opacity="0.5"
            />
            <path
              d="M56.5 16H93.5C93.5 16 101 18 93.5 27C86 36 51 27 51 27L56.5 16Z"
              fill="#88FFD4"
              fill-opacity="0.3"
            />
            <path
              d="M70 24C68 17 60 24 86 24C112 24 57.5 48 57.5 48C57.5 48 38 46 42.5 46.5C47 47 72 31 70 24Z"
              fill="#88FFD4"
              fill-opacity="0.4"
            />
          </svg> */}
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
            <Label
              style={{
                color: "red",
                cursor: "pointer",
                fontSize: ".875rem",
                wordWrap: "break-word",
                textAlign: "left",
                whiteSpace: "pre-wrap",
                overflow: "hidden",
                textOverflow: "ellipsis",
                display: "-webkit-box",
              }}
            >
              {errorMessage}
            </Label>
          )}
          <Button className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300" onClick={() => authUser()}>Войти</Button>
          <Button className="w-full text-white bg-blue-400 hover:bg-blue-600 ease-in-out transition-300" onClick={() => signUp()}>Зарегистрироваться</Button>
        </div>
      {/* )} */}
    </>
  );
}
