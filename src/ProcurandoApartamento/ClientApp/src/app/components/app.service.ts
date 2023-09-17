import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Apartamento } from './app.model';

@Injectable({ providedIn: 'root' })
export class ApartamentoService {
    private apartamentoApiUrl!: string;

    constructor(private http: HttpClient) {
        this.apartamentoApiUrl = `https://localhost:44450/api/apartamentos/melhor`;
    }

    public getMelhorApartamento(estabelecimentos: string[]): Observable<Apartamento> {
        return this.http.get<Apartamento>(this.getMelhorApartamentoUrl(estabelecimentos));
    }

    private getMelhorApartamentoUrl(estabelecimentos: string[]): string {
        if (estabelecimentos.length === 0) {
            return `${this.apartamentoApiUrl}`;
        }

        let baseUrl: string = `${this.apartamentoApiUrl}?`;

        estabelecimentos.forEach((estabelecimento) => {
            baseUrl = baseUrl.concat(`estabelecimentos=${estabelecimento}&`);
        });

        return baseUrl;
    }
}