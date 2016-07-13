using System;
using System.Linq.Expressions;

namespace Solum.Handlers
{
	static class InterpretaHandler
	{
		public static string InterpretaTextura(float argila, float silite){
			if (argila < 150) {
				return "Arenosa";
			}
			if ((argila + silite) > 150 && argila < 350) {
				return "Média";
			}
			if (argila < 600){
				return "Argilosa";
			} 
			else {
				return "Muito argilosa";
			}
		}

		public static string InterpretaPh(float ph){
			if (ph <= 4.4) {
				return "Acidez alta";
			}
			if (ph > 4.4 && ph <= 4.8) {
				return "Acidez média";
			}
			if (ph > 4.8 && ph <= 5.5 ) {
				return "Acidez adequada";
			}
			if (ph > 5.5 && ph <= 5.8) {
				return "Acidez baixa";
			} 
			else {
				return "Acidez muito baixa";
			}
		}

		public static string InterpretaP(float p, string textura){
			switch (textura) {
			case "Arenosa":
				if (p <= 6) {
					return "Muito baixo";
				}
				if (p > 6 && p <= 12) {
					return "Baixo";
				}
				if (p > 12 && p <= 18) {
					return "Médio";
				}
				if(p > 18 && p <= 25) {
					return "Adequado";
				} 
				else {
					return "Alto";
				}
			case "Média":
				if (p <= 5) {
					return "Muito baixo";
				}
				if (p > 5 && p <= 10) {
					return "Baixo";
				}
				if (p > 10 && p <= 15) {
					return "Médio";
				}
				if(p > 15 && p <= 20) {
					return "Adequado";
				} 
				else {
					return "Alto";
				}
			case "Argilosa":
				if (p <= 3) {
					return "Muito baixo";
				}
				if (p > 3 && p <= 5) {
					return "Baixo";
				}
				if (p > 5 && p <= 8) {
					return "Médio";
				}
				if(p > 8 && p <= 12) {
					return "Adequado";
				} 
				else {
					return "Alto";
				}
			case "Muito argilosa":
				if (p <= 2) {
					return "Muito baixo";
				}
				if (p > 2 && p <= 3) {
					return "Baixo";
				}
				if (p > 3 && p <= 4) {
					return "Médio";
				}
				if(p > 4 && p <= 6) {
					return "Adequado";
				} 
				else {
					return "Alto";
				}
			default:
				return "";
			}
		}

		public static string InterpretaK(float k, float ctc){
			if (ctc < 4) {
				if (k <= 15) {
					return "Baixo";
				}
				if (k > 15 && k <= 30) {
					return "Médio";
				}
				if (k > 30 && k <= 40) {
					return "Adequado";
				} 
				else {
					return "Alto";
				}
			} else {
				if (k <= 25) {
					return "Baixo";
				}
				if (k > 25 && k <= 50) {
					return "Médio";
					}
				if (k > 50 && k <= 80) {
					return "Adequado";
				} 
				else {
					return "Alto";
				}
			}
		}

		public static string InterpretaCa(float ca){
			if (ca < 1.5) {
				return "Baixo";
			}
			if (ca >= 1.5 && ca <= 7) {
				return "Adequado";
			} else {
				return "Alto";
			}
		}

		public static string InterpretaMg(float mg){
			if (mg < 0.5){
				return "Baixo";
			}
			if (mg >= 0.5 && mg <= 2) {
				return "Adequado";
			} 
			else {
				return "Alto";
			}
		}

		public static string InterpretaCaK(float caK){
			if (caK <= 7){
				return "Baixa";
			}
			if (caK > 7 && caK <=14){
				return "Média";
			}
			if (caK > 14 && caK <= 25) {
				return "Adequada";
			} 
			else {
				return "Alta";
			}
		}

		public static string InterpretaMgK(float mgK){
			if (mgK <= 2){
				return "Baixa";
			}
			if (mgK > 2 && mgK <=4){
				return "Média";
			}
			if (mgK > 4 && mgK <= 15) {
				return "Adequada";
			} 
			else {
				return "Alta";
			}
		}

		public static string InterpretaM(float m){
			if (m < 20) {
				return "Baixa";
			}
			if (m >= 20 && m <=60) {
				return "Alta";
			} else {
				return "Muito alta";
			}
		}

		public static string InterpretaV(float v){
			if (v <= 20) {
				return "Baixa";
			}
			if (v > 20 && v <= 35) {
				return "Média";
			}
			if (v > 35 && v <= 60) {
				return "Adequada";
			}
			if (v > 60 && v <= 70) {
				return "Alta";
			} 
			else {
				return "Muito alta";
			}
		}

		public static string InterpretaCtc(float ctc, string textura){
			switch (textura) {
			case "Arenosa":
				if (ctc < 3.2) {
					return "Baixa";
				}
				if (ctc >= 3.2 && ctc <= 4) {
					return "Média";
				}
				if (ctc > 4 && ctc <= 6) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			case "Média":
				if (ctc < 4.8) {
					return "Baixa";
				}
				if (ctc >= 4.8 && ctc <= 6) {
					return "Média";
				}
				if (ctc > 6 && ctc <= 9) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			case "Argilosa":
				if (ctc < 7.2) {
					return "Baixa";
				}
				if (ctc >= 7.2 && ctc <= 9) {
					return "Média";
				}
				if (ctc > 9 && ctc <= 13.5) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			case "Muito argilosa":
				if (ctc < 9.6) {
					return "Baixa";
				}
				if (ctc >= 9.6 && ctc <= 12) {
					return "Média";
				}
				if (ctc > 12 && ctc <= 18) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			default:
				return "";
			}
		}

		public static string InterpretaMo(float mo, string textura){
			switch (textura) {
			case "Arenosa":
				if (mo < 8) {
					return "Baixa";
				}
				if (mo >= 8 && mo <= 10) {
					return "Média";
				}
				if (mo > 10 && mo <= 15) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			case "Média":
				if (mo < 16) {
					return "Baixa";
				}
				if (mo >= 16 && mo <= 20) {
					return "Média";
				}
				if (mo > 20 && mo <= 30) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			case "Argilosa":
				if (mo < 24) {
					return "Baixa";
				}
				if (mo >= 24 && mo <= 30) {
					return "Média";
				}
				if (mo > 30 && mo <= 45) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			case "Muito argilosa":
				if (mo < 28) {
					return "Baixa";
				}
				if (mo >= 8 && mo <= 35) {
					return "Média";
				}
				if (mo > 35 && mo <= 52) {
					return "Adequada";
				}
				else {
					return "Alta";
				}
			default:
				return "";
			}
		}
	}
}

