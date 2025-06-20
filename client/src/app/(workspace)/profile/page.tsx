import ProfileSettings from "@/components/profileSettings"


export default function Page(){
    return(
        <>
            <div className="w-full h-full pl-14 pr-14 pt-6 pb-6 flex self-center flex-col items-start">
                <div className="border-b pb-4 w-full">
                    <h1 className="text-[1.8rem]">Личный кабинет</h1>   
                    <h3 className="text-[0.9rem]">Здесь вы можете изменить персональную информацию о себе</h3>
                </div>
                <ProfileSettings/>
            </div>
        </>
    )
}