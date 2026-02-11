

export class EnumUtils {

    static getEnumNumbers<T>(enumObject: { [p: string]: string | T }): T[] {
        const values = Object.values(enumObject)
        return values.slice(values.length / 2)
            .map(val => val as T);
    }

    static getEnumValues<T>(enumObject: { [p: string]: string | T }): T[] {
        const values = Object.values(enumObject)
        return values.slice(0, values.length / 2)
            .map(val => val as T);
    }
}